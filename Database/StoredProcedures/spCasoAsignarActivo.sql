-- =============================================
-- Stored Procedure: spCasoAsignarActivo
-- Descripción: Asignar activo a un caso existente
-- =============================================

CREATE OR ALTER PROCEDURE soporte.spCasoAsignarActivo
    @IdCaso BIGINT,
    @IdActivo BIGINT,
    @IdUsuarioAccion BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @NumeroCaso VARCHAR(50);
        DECLARE @Descripcion VARCHAR(MAX);
        DECLARE @IdEstadoCaso BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @IdTecnicoAsignado BIGINT;
        DECLARE @NombreActivo VARCHAR(200);
        DECLARE @NombreEstadoCaso VARCHAR(100);

        -- 1) Validar que el caso existe
        IF NOT EXISTS (SELECT 1 FROM soporte.Caso WHERE IdCaso = @IdCaso)
            THROW 50001, 'El caso no existe.', 1;

        -- 2) Obtener información del caso
        SELECT 
            @NumeroCaso = NumeroCaso,
            @Descripcion = Descripcion,
            @IdEstadoCaso = IdEstadoCaso,
            @IdAreaTecnica = IdAreaTecnica,
            @IdTecnicoAsignado = IdTecnicoAsignado
        FROM soporte.Caso
        WHERE IdCaso = @IdCaso;

        -- 3) Validar que el caso no esté cerrado o rechazado
        SELECT @NombreEstadoCaso = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCaso;

        IF @NombreEstadoCaso IN ('Cerrado', 'Rechazado')
            THROW 50003, 'No se puede asignar activo a un caso cerrado o rechazado.', 1;

        -- 4) Validar que el activo existe
        SELECT @NombreActivo = NombreActivo
        FROM inventario.Activo
        WHERE IdActivo = @IdActivo;

        IF @NombreActivo IS NULL
            THROW 50002, 'El activo no existe.', 1;

        -- 5) Actualizar caso con el activo
        UPDATE soporte.Caso
        SET 
            IdActivo = @IdActivo,
            FechaActualizacion = SYSUTCDATETIME()
        WHERE IdCaso = @IdCaso;

        -- 6) Crear entrada en HojaDeVidaActivo
        INSERT INTO inventario.HojaDeVidaActivo (
            IdActivo,
            DetalleRegistro,
            TipoEvento,
            IdCaso,
            IdUsuarioCreacion
        )
        VALUES (
            @IdActivo,
            'Activo asociado al caso ' + @NumeroCaso + '. ' + LEFT(@Descripcion, 200),
            'ActivoAsignado',
            @IdCaso,
            @IdUsuarioAccion
        );

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
            'ActivoAsignado',
            'Activo asignado: ' + @NombreActivo,
            @IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        COMMIT TRANSACTION;

        -- 8) Retornar caso actualizado
        SELECT
            c.IdCaso,
            c.NumeroCaso,
            c.IdActivo,
            a.NombreActivo,
            a.Serie,
            a.CodigoPatrimonial,
            c.FechaActualizacion
        FROM soporte.Caso c
        INNER JOIN inventario.Activo a ON c.IdActivo = a.IdActivo
        WHERE c.IdCaso = @IdCaso;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
