CREATE OR ALTER PROCEDURE soporte.spIntervencionTecnicaCrearConDiagnostico
    @IdCaso BIGINT,
    @IdTipoTrabajo BIGINT,
    @Diagnostico VARCHAR(MAX),
    @IdUsuarioAccion BIGINT,
    @IdIntervencionNueva BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdTecnicoAsignado BIGINT;
        DECLARE @IdActivo BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @NumeroCaso VARCHAR(50);
        DECLARE @IdEstadoCaso BIGINT;
        DECLARE @NombreEstadoCaso VARCHAR(100);
        DECLARE @IdTrazabilidadCasoNueva BIGINT;
        DECLARE @IdEstadoPendienteAprobacion BIGINT;
        DECLARE @IdEstadoEnProgreso BIGINT;

        IF NOT EXISTS (SELECT 1 FROM soporte.Caso WHERE IdCaso = @IdCaso)
            THROW 50001, 'El caso no existe.', 1;

        SELECT
            @IdTecnicoAsignado = IdTecnicoAsignado,
            @IdActivo = IdActivo,
            @IdAreaTecnica = IdAreaTecnica,
            @NumeroCaso = NumeroCaso,
            @IdEstadoCaso = IdEstadoCaso
        FROM soporte.Caso
        WHERE IdCaso = @IdCaso;

        SELECT @NombreEstadoCaso = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCaso;

        IF @NombreEstadoCaso IN ('Cerrado', 'Rechazado')
            THROW 50007, 'No se puede crear intervencion en un caso cerrado o rechazado.', 1;

        IF @IdTecnicoAsignado IS NULL
            THROW 50008, 'El caso no tiene tecnico asignado.', 1;

        IF @IdUsuarioAccion <> @IdTecnicoAsignado
            THROW 50002, 'Solo el tecnico asignado puede crear intervencion.', 1;

        SELECT @IdEstadoPendienteAprobacion = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Pendiente Aprobacion';

        SELECT @IdEstadoEnProgreso = IdEstadoCaso
        FROM catalogo.EstadoCaso
        WHERE NombreEstadoCaso = 'En Progreso';

        IF @IdEstadoPendienteAprobacion IS NULL
            THROW 50009, 'No existe el estado "Pendiente Aprobacion" en catalogo.EstadoIntervencionTecnica.', 1;

        IF @IdEstadoEnProgreso IS NULL
            THROW 50010, 'No existe el estado "En Progreso" en catalogo.EstadoCaso.', 1;

        UPDATE soporte.Caso
        SET
            IdEstadoCaso = CASE
                WHEN @NombreEstadoCaso IN ('Asignado', 'Abierto') THEN @IdEstadoEnProgreso
                ELSE IdEstadoCaso
            END,
            FechaActualizacion = SYSUTCDATETIME()
        WHERE IdCaso = @IdCaso;

        SET @IdEstadoCaso = CASE
            WHEN @NombreEstadoCaso IN ('Asignado', 'Abierto') THEN @IdEstadoEnProgreso
            ELSE @IdEstadoCaso
        END;

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
            'DiagnosticoRegistrado',
            'Diagnostico registrado para intervencion: ' + LEFT(@Diagnostico, 100),
            @IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        SET @IdTrazabilidadCasoNueva = SCOPE_IDENTITY();

        INSERT INTO soporte.IntervencionTecnica (
            IdTrazabilidadCaso,
            IdTipoTrabajo,
            IdEstadoIntervencion,
            FechaInicio,
            Diagnostico,
            SolucionAplicada,
            FechaFin,
            IdUsuarioAccion
        )
        VALUES (
            @IdTrazabilidadCasoNueva,
            @IdTipoTrabajo,
            @IdEstadoPendienteAprobacion,
            SYSUTCDATETIME(),
            @Diagnostico,
            NULL,
            NULL,
            @IdUsuarioAccion
        );

        SET @IdIntervencionNueva = SCOPE_IDENTITY();

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
                'Diagnostico registrado: ' + LEFT(@Diagnostico, 200),
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
        WHERE i.IdIntervencionTecnica = @IdIntervencionNueva;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
