CREATE OR ALTER PROCEDURE soporte.spCasoAsignarTecnico
    @IdCaso BIGINT,
    @IdTecnicoAsignado BIGINT,
    @IdUsuarioAccion BIGINT,
    @Comentario VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdEstadoAsignado BIGINT;
        DECLARE @NombreTecnico VARCHAR(150);
        DECLARE @TecnicoAnterior BIGINT;
        DECLARE @NombreTecnicoAnterior VARCHAR(150);
        DECLARE @EsReasignacion BIT = 0;
        DECLARE @ComentarioFinal VARCHAR(500);
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @TipoEvento VARCHAR(100);
        DECLARE @IdEstadoCasoActual BIGINT;
        DECLARE @NombreEstadoActual VARCHAR(100);

        -- 1) Validar que el caso existe
        IF NOT EXISTS (SELECT 1 FROM soporte.Caso WHERE IdCaso = @IdCaso)
            THROW 50001, 'El caso no existe.', 1;

        -- 2) Traer info base del caso
        SELECT
            @TecnicoAnterior = c.IdTecnicoAsignado,
            @IdAreaTecnica = c.IdAreaTecnica,
            @IdEstadoCasoActual = c.IdEstadoCaso
        FROM soporte.Caso c
        WHERE c.IdCaso = @IdCaso;

        -- 3) Validar que el caso no esté cerrado o rechazado
        SELECT @NombreEstadoActual = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCasoActual;

        IF @NombreEstadoActual IN ('Cerrado', 'Rechazado')
            THROW 50005, 'No se puede asignar técnico a un caso cerrado o rechazado.', 1;

        -- 4) Validar que el tecnico existe y tiene rol adecuado
        IF NOT EXISTS (
            SELECT 1
            FROM acceso.Usuario u
            INNER JOIN acceso.Rol r ON u.IdRol = r.IdRol
            WHERE u.IdUsuario = @IdTecnicoAsignado
              AND r.NombreRol IN ('Tecnico', 'Administrador')
        )
            THROW 50002, 'El usuario no es tecnico o no existe.', 1;

        -- 5) Obtener estado "Asignado"
        SELECT @IdEstadoAsignado = IdEstadoCaso
        FROM catalogo.EstadoCaso
        WHERE NombreEstadoCaso = 'Asignado';

        IF @IdEstadoAsignado IS NULL
            THROW 50003, 'No existe el estado Asignado en catalogo.EstadoCaso.', 1;

        -- 6) Nombre del tecnico nuevo
        SELECT @NombreTecnico = NombreCompleto
        FROM acceso.Usuario
        WHERE IdUsuario = @IdTecnicoAsignado;

        IF @NombreTecnico IS NULL
            THROW 50004, 'El tecnico asignado no existe en acceso.Usuario.', 1;

        -- 7) Idempotencia: si ya estaba asignado al mismo tecnico, no hacer nada
        IF @TecnicoAnterior = @IdTecnicoAsignado
        BEGIN
            COMMIT TRANSACTION;

            SELECT
                c.IdCaso,
                c.NumeroCaso,
                c.Descripcion,
                c.IdTecnicoAsignado,
                u.NombreCompleto AS NombreTecnicoAsignado,
                c.IdEstadoCaso,
                ec.NombreEstadoCaso,
                c.FechaAceptacion,
                c.FechaActualizacion
            FROM soporte.Caso c
            INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
            LEFT JOIN acceso.Usuario u ON c.IdTecnicoAsignado = u.IdUsuario
            WHERE c.IdCaso = @IdCaso;

            RETURN;
        END

        -- 8) Reasignacion si habia tecnico previo
        IF @TecnicoAnterior IS NOT NULL
        BEGIN
            SET @EsReasignacion = 1;

            SELECT @NombreTecnicoAnterior = NombreCompleto
            FROM acceso.Usuario
            WHERE IdUsuario = @TecnicoAnterior;
        END

        -- 9) Actualizar caso
        UPDATE soporte.Caso
        SET
            IdTecnicoAsignado = @IdTecnicoAsignado,
            FechaAceptacion = CASE WHEN FechaAceptacion IS NULL THEN SYSUTCDATETIME() ELSE FechaAceptacion END,
            IdEstadoCaso = @IdEstadoAsignado,
            FechaActualizacion = SYSUTCDATETIME()
        WHERE IdCaso = @IdCaso;

        -- 10) Trazabilidad automatica (evento correcto)
        SET @TipoEvento = CASE WHEN @EsReasignacion = 1 THEN 'TecnicoReasignado' ELSE 'TecnicoAsignado' END;

        IF @EsReasignacion = 1
            SET @ComentarioFinal = COALESCE(
                @Comentario,
                'Caso reasignado de ' + COALESCE(@NombreTecnicoAnterior,'(desconocido)') + ' a ' + @NombreTecnico
            );
        ELSE
            SET @ComentarioFinal = COALESCE(
                @Comentario,
                'Asignado a tecnico: ' + @NombreTecnico
            );

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
            @TipoEvento,
            @ComentarioFinal,
            @IdEstadoAsignado,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        COMMIT TRANSACTION;

        -- 11) Retornar caso actualizado
        SELECT
            c.IdCaso,
            c.NumeroCaso,
            c.Descripcion,
            c.IdTecnicoAsignado,
            u.NombreCompleto AS NombreTecnicoAsignado,
            c.IdEstadoCaso,
            ec.NombreEstadoCaso,
            c.FechaAceptacion,
            c.FechaActualizacion
        FROM soporte.Caso c
        INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
        LEFT JOIN acceso.Usuario u ON c.IdTecnicoAsignado = u.IdUsuario
        WHERE c.IdCaso = @IdCaso;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO