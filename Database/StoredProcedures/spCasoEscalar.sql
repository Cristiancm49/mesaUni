CREATE OR ALTER PROCEDURE soporte.spCasoEscalar
    @IdCaso BIGINT,
    @IdAreaTecnicaNueva BIGINT,
    @IdTecnicoAsignadoNuevo BIGINT = NULL,
    @MotivoEscalamiento VARCHAR(500),
    @IdUsuarioAccion BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdEstadoEscalado BIGINT;
        DECLARE @IdAreaTecnicaAnterior BIGINT;
        DECLARE @IdTecnicoAnterior BIGINT;
        DECLARE @NombreAreaAnterior VARCHAR(150) = NULL;
        DECLARE @NombreAreaNueva VARCHAR(150);
        DECLARE @NombreTecnicoNuevo VARCHAR(150) = NULL;
        DECLARE @ComentarioFinal VARCHAR(500);
        DECLARE @IdTrazabilidadCaso BIGINT;
        DECLARE @IdEstadoCasoActual BIGINT;
        DECLARE @NombreEstadoActual VARCHAR(100);

        -- 1) Validar que el caso existe
        IF NOT EXISTS (SELECT 1 FROM soporte.Caso WHERE IdCaso = @IdCaso)
            THROW 50001, 'El caso no existe.', 1;

        -- 2) Traer datos actuales del caso
        SELECT
            @IdAreaTecnicaAnterior = IdAreaTecnica,
            @IdTecnicoAnterior = IdTecnicoAsignado,
            @IdEstadoCasoActual = IdEstadoCaso
        FROM soporte.Caso
        WHERE IdCaso = @IdCaso;

        -- 3) Validar que el caso no esté cerrado o rechazado
        SELECT @NombreEstadoActual = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCasoActual;

        IF @NombreEstadoActual IN ('Cerrado', 'Rechazado')
            THROW 50005, 'No se puede escalar un caso cerrado o rechazado.', 1;

        -- 4) Validar que el área técnica nueva existe
        SELECT @NombreAreaNueva = NombreAreaTecnica
        FROM catalogo.AreaTecnica
        WHERE IdAreaTecnica = @IdAreaTecnicaNueva;

        IF @NombreAreaNueva IS NULL
            THROW 50002, 'El área técnica destino no existe.', 1;

        -- 5) Validar que el área técnica nueva sea diferente a la actual
        IF @IdAreaTecnicaAnterior IS NOT NULL AND @IdAreaTecnicaAnterior = @IdAreaTecnicaNueva
            THROW 50007, 'El área técnica destino debe ser diferente a la actual.', 1;

        -- 6) Nombre del área anterior (si aplica)
        IF @IdAreaTecnicaAnterior IS NOT NULL
        BEGIN
            SELECT @NombreAreaAnterior = NombreAreaTecnica
            FROM catalogo.AreaTecnica
            WHERE IdAreaTecnica = @IdAreaTecnicaAnterior;
        END

        -- 7) Validar motivo (si no lo obligas, esto se vuelve basura)
        IF @MotivoEscalamiento IS NULL OR LTRIM(RTRIM(@MotivoEscalamiento)) = ''
            THROW 50006, 'Debe especificar el motivo del escalamiento.', 1;

        -- 8) Si se especifica técnico nuevo, validar que existe y rol válido (según tus roles reales)
        IF @IdTecnicoAsignadoNuevo IS NOT NULL
        BEGIN
            IF NOT EXISTS (
                SELECT 1
                FROM acceso.Usuario u
                INNER JOIN acceso.Rol r ON u.IdRol = r.IdRol
                WHERE u.IdUsuario = @IdTecnicoAsignadoNuevo
                  AND r.NombreRol IN ('Tecnico', 'Administrador')
            )
                THROW 50003, 'El técnico asignado no existe o no tiene rol adecuado.', 1;

            SELECT @NombreTecnicoNuevo = NombreCompleto
            FROM acceso.Usuario
            WHERE IdUsuario = @IdTecnicoAsignadoNuevo;
        END

        -- 9) Obtener estado "Escalado"
        SELECT @IdEstadoEscalado = IdEstadoCaso
        FROM catalogo.EstadoCaso
        WHERE NombreEstadoCaso = 'Escalado';

        IF @IdEstadoEscalado IS NULL
            THROW 50004, 'No existe el estado Escalado en catalogo.EstadoCaso.', 1;

        -- 10) Actualizar caso
        UPDATE soporte.Caso
        SET
            IdAreaTecnica = @IdAreaTecnicaNueva,
            IdTecnicoAsignado = @IdTecnicoAsignadoNuevo,
            IdEstadoCaso = @IdEstadoEscalado,
            FechaActualizacion = SYSUTCDATETIME()
        WHERE IdCaso = @IdCaso;

        -- 11) Comentario final consistente
        SET @ComentarioFinal = 'Caso escalado';

        IF @NombreAreaAnterior IS NOT NULL
            SET @ComentarioFinal = @ComentarioFinal + ' de ' + @NombreAreaAnterior;

        SET @ComentarioFinal = @ComentarioFinal + ' a ' + @NombreAreaNueva;

        IF @IdTecnicoAsignadoNuevo IS NOT NULL
            SET @ComentarioFinal = @ComentarioFinal + '. Reasignado a: ' + @NombreTecnicoNuevo;
        ELSE
            SET @ComentarioFinal = @ComentarioFinal + '. Queda sin tecnico asignado';

        SET @ComentarioFinal = @ComentarioFinal + '. Motivo: ' + @MotivoEscalamiento;

        -- 12) Trazabilidad (tu CHECK permite CasoEscalado)
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
            'CasoEscalado',
            @ComentarioFinal,
            @IdEstadoEscalado,
            @IdAreaTecnicaNueva,
            @IdTecnicoAsignadoNuevo
        );
        SET @IdTrazabilidadCaso = SCOPE_IDENTITY();
        COMMIT TRANSACTION;

        -- 13) Retornar caso actualizado
        SELECT
            c.IdCaso,
            c.NumeroCaso,
            c.Descripcion,
            c.IdEstadoCaso,
            ec.NombreEstadoCaso,
            c.IdAreaTecnica,
            atc.NombreAreaTecnica,
            c.IdTecnicoAsignado,
            u.NombreCompleto AS NombreTecnicoAsignado,
            c.FechaActualizacion,

            t.IdTrazabilidadCaso,
            t.TipoEvento,
            t.Comentario AS MotivoEscalamiento
        FROM soporte.Caso c
        INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
        LEFT JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
        LEFT JOIN acceso.Usuario u ON c.IdTecnicoAsignado = u.IdUsuario
        INNER JOIN soporte.TrazabilidadCaso t ON t.IdTrazabilidadCaso = @IdTrazabilidadCaso
        WHERE c.IdCaso = @IdCaso;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
