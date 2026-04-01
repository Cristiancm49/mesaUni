
CREATE OR ALTER PROCEDURE soporte.spEncuestaCalidadCrear
    @IdCaso BIGINT,
    @Observaciones VARCHAR(MAX) = NULL,
    @IdUsuarioCreacion BIGINT,
    @RespuestasJSON VARCHAR(MAX),  -- [{"IdPregunta":1,"IdRespuesta":5,"ValorNumerico":5},...]
    @IdEncuestaNueva BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdEstadoCaso BIGINT;
        DECLARE @NombreEstadoCaso VARCHAR(100);
        DECLARE @IdUsuarioReporta BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @IdTecnicoAsignado BIGINT;

        -- 1) Validar que el caso existe
        IF NOT EXISTS (SELECT 1 FROM soporte.Caso WHERE IdCaso = @IdCaso)
            THROW 50001, 'El caso no existe.', 1;

        -- 2) Obtener información del caso
        SELECT 
            @IdEstadoCaso = IdEstadoCaso,
            @IdUsuarioReporta = IdUsuarioReporta,
            @IdAreaTecnica = IdAreaTecnica,
            @IdTecnicoAsignado = IdTecnicoAsignado
        FROM soporte.Caso
        WHERE IdCaso = @IdCaso;

        -- 3) Validar que el caso está cerrado
        SELECT @NombreEstadoCaso = NombreEstadoCaso
        FROM catalogo.EstadoCaso
        WHERE IdEstadoCaso = @IdEstadoCaso;

        IF @NombreEstadoCaso NOT IN ('Resuelto', 'Cerrado')
            THROW 50002, 'Solo se puede crear encuesta para casos resueltos o cerrados.', 1;

        -- 4) Validar que solo el usuario que reportó puede crear la encuesta
        IF @IdUsuarioCreacion <> @IdUsuarioReporta
            THROW 50003, 'Solo el usuario que reportó puede crear la encuesta.', 1;

        -- 5) Validar que no exista encuesta previa
        IF EXISTS (SELECT 1 FROM soporte.EncuestaCalidad WHERE IdCaso = @IdCaso)
            THROW 50004, 'Ya existe una encuesta para este caso.', 1;

        -- 6) Insertar encuesta
        INSERT INTO soporte.EncuestaCalidad (
            IdCaso,
            Observaciones,
            IdUsuarioCreacion
        )
        VALUES (
            @IdCaso,
            @Observaciones,
            @IdUsuarioCreacion
        );

        SET @IdEncuestaNueva = SCOPE_IDENTITY();

        -- 7) Insertar detalles de respuestas
        INSERT INTO soporte.DetalleEncuesta (
            IdEncuesta,
            IdPregunta,
            IdRespuesta
        )
        SELECT 
            @IdEncuestaNueva,
            IdPregunta,
            IdRespuesta
        FROM OPENJSON(@RespuestasJSON) WITH (
            IdPregunta BIGINT '$.IdPregunta',
            IdRespuesta BIGINT '$.IdRespuesta'
        );

        -- 8) TRAZABILIDAD AUTOMÁTICA
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
            @IdUsuarioCreacion,
            'EncuestaCreada',
            'Encuesta de calidad completada. ' + COALESCE(@Observaciones, ''),
            @IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        COMMIT TRANSACTION;

        -- 9) Retornar encuesta creada con detalles
        SELECT
            e.IdEncuesta AS Id,
            e.IdCaso,
            c.NumeroCaso,
            e.FechaEncuesta,
            e.Observaciones,
            e.IdUsuarioCreacion,
            u.NombreCompleto AS NombreUsuario,
            (SELECT COUNT(*) FROM soporte.DetalleEncuesta WHERE IdEncuesta = e.IdEncuesta) AS CantidadRespuestas
        FROM soporte.EncuestaCalidad e
        INNER JOIN soporte.Caso c ON e.IdCaso = c.IdCaso
        INNER JOIN acceso.Usuario u ON e.IdUsuarioCreacion = u.IdUsuario
        WHERE e.IdEncuesta = @IdEncuestaNueva;

        -- Result Set 2: Respuestas de la encuesta
        SELECT
            de.IdDetalleEncuesta AS Id,
            de.IdPregunta,
            p.TextoPregunta,
            de.IdRespuesta,
            r.TextoRespuesta,
            r.ValorNumerico,
            de.FechaRegistro
        FROM soporte.DetalleEncuesta de
        INNER JOIN catalogo.Pregunta p ON de.IdPregunta = p.IdPregunta
        LEFT JOIN catalogo.Respuesta r ON de.IdRespuesta = r.IdRespuesta
        WHERE de.IdEncuesta = @IdEncuestaNueva;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
