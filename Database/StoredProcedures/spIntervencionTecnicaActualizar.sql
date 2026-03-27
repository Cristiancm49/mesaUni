-- =============================================
-- Stored Procedure: spIntervencionTecnicaActualizar
-- Descripción: Actualizar intervención técnica existente (solo si no ha sido revisada)
-- =============================================

CREATE OR ALTER PROCEDURE soporte.spIntervencionTecnicaActualizar
    @IdIntervencionTecnica BIGINT,
    @IdTipoTrabajo BIGINT = NULL,
    @IdEstadoIntervencion BIGINT = NULL,
    @FechaInicio DATETIME = NULL,
    @FechaFin DATETIME = NULL,
    @Diagnostico VARCHAR(MAX) = NULL,
    @SolucionAplicada VARCHAR(MAX) = NULL,
    @IdUsuarioAccion BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdCaso BIGINT;
        DECLARE @IdTrazabilidadCaso BIGINT;
        DECLARE @IdTecnicoAsignado BIGINT;
        DECLARE @IdEstadoCaso BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @NombreEstadoCaso VARCHAR(100);

        -- 1) Validar que la intervención existe y obtener info
        SELECT 
            @IdTrazabilidadCaso = i.IdTrazabilidadCaso,
            @IdCaso = t.IdCaso
        FROM soporte.IntervencionTecnica i
        INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
        WHERE i.IdIntervencionTecnica = @IdIntervencionTecnica;

        IF @IdCaso IS NULL
            THROW 50001, 'La intervención técnica no existe.', 1;

        -- 2) Validar que no ha sido revisada
        IF EXISTS (
            SELECT 1 FROM soporte.RevisionAdmi 
            WHERE IdIntervencionTecnica = @IdIntervencionTecnica
        )
            THROW 50002, 'No se puede actualizar una intervención que ya fue revisada.', 1;

        -- 3) Obtener información del caso
        SELECT 
            @IdTecnicoAsignado = IdTecnicoAsignado,
            @IdEstadoCaso = IdEstadoCaso,
            @IdAreaTecnica = IdAreaTecnica
        FROM soporte.Caso
        WHERE IdCaso = @IdCaso;

        -- 4) Validar que el caso no esté cerrado o rechazado
        SELECT @NombreEstadoCaso = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCaso;

        IF @NombreEstadoCaso IN ('Cerrado', 'Rechazado')
            THROW 50004, 'No se puede actualizar intervención de un caso cerrado o rechazado.', 1;

        -- 5) Validar que solo el técnico asignado puede actualizar
        IF @IdUsuarioAccion <> @IdTecnicoAsignado
            THROW 50003, 'Solo el técnico asignado puede actualizar la intervención.', 1;

        -- 6) Actualizar intervención (solo campos no nulos)
        UPDATE soporte.IntervencionTecnica
        SET 
            IdTipoTrabajo = COALESCE(@IdTipoTrabajo, IdTipoTrabajo),
            IdEstadoIntervencion = COALESCE(@IdEstadoIntervencion, IdEstadoIntervencion),
            FechaInicio = COALESCE(@FechaInicio, FechaInicio),
            FechaFin = COALESCE(@FechaFin, FechaFin),
            Diagnostico = COALESCE(@Diagnostico, Diagnostico),
            SolucionAplicada = COALESCE(@SolucionAplicada, SolucionAplicada)
        WHERE IdIntervencionTecnica = @IdIntervencionTecnica;

        -- 7) TRAZABILIDAD AUTOMÁTICA
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
            'IntervencionActualizada',
            'Intervención técnica actualizada',
            @IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        COMMIT TRANSACTION;

        -- 8) Retornar intervención actualizada
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
