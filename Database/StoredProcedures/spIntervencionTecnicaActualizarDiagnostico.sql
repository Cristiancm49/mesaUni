CREATE OR ALTER PROCEDURE soporte.spIntervencionTecnicaActualizarDiagnostico
    @IdIntervencionTecnica BIGINT,
    @IdTipoTrabajo BIGINT = NULL,
    @Diagnostico VARCHAR(MAX),
    @IdUsuarioAccion BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdCaso BIGINT;
        DECLARE @IdTrazabilidadCaso BIGINT;
        DECLARE @IdEstadoPendienteAprobacion BIGINT;
        DECLARE @IdEstadoIntervencionActual BIGINT;
        DECLARE @NombreEstadoIntervencionActual VARCHAR(100);
        DECLARE @IdTecnicoAsignado BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @IdEstadoCaso BIGINT;
        DECLARE @IdActivo BIGINT;

        SELECT
            @IdTrazabilidadCaso = i.IdTrazabilidadCaso,
            @IdEstadoIntervencionActual = i.IdEstadoIntervencion
        FROM soporte.IntervencionTecnica i
        WHERE i.IdIntervencionTecnica = @IdIntervencionTecnica;

        IF @IdTrazabilidadCaso IS NULL
            THROW 50001, 'La intervencion tecnica no existe.', 1;

        SELECT @IdCaso = t.IdCaso
        FROM soporte.TrazabilidadCaso t
        WHERE t.IdTrazabilidadCaso = @IdTrazabilidadCaso;

        SELECT @NombreEstadoIntervencionActual = eit.NombreEstado
        FROM catalogo.EstadoIntervencionTecnica eit
        WHERE eit.IdEstadoIntervencion = @IdEstadoIntervencionActual;

        IF @NombreEstadoIntervencionActual NOT IN ('Rechazada', 'Pendiente Aprobacion')
            THROW 50002, 'Solo se puede corregir un diagnostico rechazado o reenviar uno pendiente de aprobacion.', 1;

        SELECT
            @IdTecnicoAsignado = c.IdTecnicoAsignado,
            @IdAreaTecnica = c.IdAreaTecnica,
            @IdEstadoCaso = c.IdEstadoCaso,
            @IdActivo = c.IdActivo
        FROM soporte.Caso c
        WHERE c.IdCaso = @IdCaso;

        IF @IdTecnicoAsignado IS NULL OR @IdUsuarioAccion <> @IdTecnicoAsignado
            THROW 50003, 'Solo el tecnico asignado puede actualizar el diagnostico.', 1;

        SELECT @IdEstadoPendienteAprobacion = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Pendiente Aprobacion';

        IF @IdEstadoPendienteAprobacion IS NULL
            THROW 50004, 'No existe el estado "Pendiente Aprobacion".', 1;

        UPDATE soporte.IntervencionTecnica
        SET
            IdTipoTrabajo = COALESCE(@IdTipoTrabajo, IdTipoTrabajo),
            Diagnostico = @Diagnostico,
            IdEstadoIntervencion = @IdEstadoPendienteAprobacion
        WHERE IdIntervencionTecnica = @IdIntervencionTecnica;

        UPDATE soporte.Caso
        SET FechaActualizacion = SYSUTCDATETIME()
        WHERE IdCaso = @IdCaso;

        INSERT INTO soporte.TrazabilidadCaso (
            IdCaso,
            FechaEvento,
            IdUsuarioAccion,
            TipoEvento,
            Comentario,
            IdEstadoCaso,
            IdAreaTecnica,
            IdTecnicoAsignado
        )
        VALUES (
            @IdCaso,
            SYSUTCDATETIME(),
            @IdUsuarioAccion,
            'DiagnosticoActualizado',
            'Diagnostico corregido y reenviado a revision: ' + LEFT(@Diagnostico, 100),
            @IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        IF @IdActivo IS NOT NULL
        BEGIN
            INSERT INTO inventario.HojaDeVidaActivo (
                IdActivo,
                FechaRegistro,
                DetalleRegistro,
                TipoEvento,
                IdCaso,
                IdUsuarioCreacion
            )
            VALUES (
                @IdActivo,
                SYSUTCDATETIME(),
                'Diagnostico actualizado: ' + LEFT(@Diagnostico, 200),
                'Mantenimiento',
                @IdCaso,
                @IdUsuarioAccion
            );
        END

        COMMIT TRANSACTION;

        SELECT
            i.IdIntervencionTecnica,
            i.IdTrazabilidadCaso,
            t.IdCaso,
            c.NumeroCaso,
            i.IdTipoTrabajo,
            tt.NombreTipoTrabajo,
            i.IdEstadoIntervencion,
            ei.NombreEstado AS NombreEstadoIntervencion,
            i.FechaInicio,
            i.FechaFin,
            i.Diagnostico,
            i.SolucionAplicada,
            i.IdUsuarioAccion,
            u.NombreCompleto AS NombreTecnico
        FROM soporte.IntervencionTecnica i
        INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
        INNER JOIN soporte.Caso c ON t.IdCaso = c.IdCaso
        INNER JOIN catalogo.TipoTrabajo tt ON i.IdTipoTrabajo = tt.IdTipoTrabajo
        INNER JOIN catalogo.EstadoIntervencionTecnica ei ON i.IdEstadoIntervencion = ei.IdEstadoIntervencion
        INNER JOIN acceso.Usuario u ON i.IdUsuarioAccion = u.IdUsuario
        WHERE i.IdIntervencionTecnica = @IdIntervencionTecnica;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
