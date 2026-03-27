CREATE OR ALTER PROCEDURE soporte.spCasoCambiarEstado
    @IdCaso BIGINT,
    @IdEstadoNuevo BIGINT,
    @IdUsuarioAccion BIGINT,
    @Comentario VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdEstadoActual BIGINT;
        DECLARE @NombreEstadoActual VARCHAR(100);
        DECLARE @NombreEstadoNuevo VARCHAR(100);
        DECLARE @IdTecnicoAsignado BIGINT;
        DECLARE @IdUsuarioReporta BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @ComentarioFinal VARCHAR(500);
        DECLARE @EsAdmin BIT = 0;
        DECLARE @TipoEvento VARCHAR(100);

        -- 1) Obtener info del caso
        SELECT
            @IdEstadoActual = c.IdEstadoCaso,
            @IdTecnicoAsignado = c.IdTecnicoAsignado,
            @IdUsuarioReporta = c.IdUsuarioReporta,
            @IdAreaTecnica = c.IdAreaTecnica
        FROM soporte.Caso c
        WHERE c.IdCaso = @IdCaso;

        IF @IdEstadoActual IS NULL
            THROW 50001, 'El caso no existe.', 1;

        -- 2) Obtener nombres de estado actual y nuevo
        SELECT @NombreEstadoActual = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoActual;

        SELECT @NombreEstadoNuevo = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoNuevo;

        IF @NombreEstadoNuevo IS NULL
            THROW 50002, 'El estado destino no existe.', 1;

        -- 3) Idempotente
        IF @IdEstadoActual = @IdEstadoNuevo
        BEGIN
            COMMIT TRANSACTION;

            SELECT
                c.IdCaso,
                c.NumeroCaso,
                c.Descripcion,
                c.IdEstadoCaso,
                ec.NombreEstadoCaso,
                c.FechaAceptacion,
                c.FechaResolucion,
                c.FechaCierre,
                c.FechaActualizacion,
                c.IdTecnicoAsignado,
                u.NombreCompleto AS NombreTecnicoAsignado
            FROM soporte.Caso c
            INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
            LEFT JOIN acceso.Usuario u ON c.IdTecnicoAsignado = u.IdUsuario
            WHERE c.IdCaso = @IdCaso;

            RETURN;
        END

        -- 4) No se puede modificar un caso cerrado o rechazado
        IF @NombreEstadoActual IN ('Cerrado', 'Rechazado')
            THROW 50008, 'No se puede modificar un caso cerrado o rechazado.', 1;

        -- 5) Determinar si el actor es Administrador
        IF EXISTS (
            SELECT 1
            FROM acceso.Usuario u
            INNER JOIN acceso.Rol r ON u.IdRol = r.IdRol
            WHERE u.IdUsuario = @IdUsuarioAccion
              AND r.NombreRol = 'Administrador'
        )
        BEGIN
            SET @EsAdmin = 1;
        END

        -- 6.1) En Progreso / Resuelto: requiere tecnico y solo el tecnico asignado puede hacerlo
        IF @NombreEstadoNuevo IN ('En Progreso', 'Resuelto')
        BEGIN
            IF @IdTecnicoAsignado IS NULL
                THROW 50006, 'No hay tecnico asignado al caso.', 1;

            IF @IdUsuarioAccion <> @IdTecnicoAsignado
                THROW 50007, 'Solo el tecnico asignado puede cambiar a este estado.', 1;
        END

        -- 6.2) Cerrado: solo si venia de Resuelto
        IF @NombreEstadoNuevo = 'Cerrado' AND @NombreEstadoActual <> 'Resuelto'
            THROW 50004, 'Solo se puede cerrar un caso resuelto.', 1;

        -- 6.3) Cerrado: lo cierra el reportante o un Administrador
        IF @NombreEstadoNuevo = 'Cerrado' AND @IdUsuarioAccion <> @IdUsuarioReporta AND @EsAdmin = 0
            THROW 50005, 'Solo el usuario que reporto o un administrador puede cerrar el caso.', 1;

        -- 6.4) Rechazado: solo Administrador y con motivo
        IF @NombreEstadoNuevo = 'Rechazado'
        BEGIN
            IF @EsAdmin = 0
                THROW 50020, 'Solo un administrador puede rechazar un caso.', 1;

            IF @NombreEstadoActual IN ('Resuelto', 'Cerrado')
                THROW 50021, 'No se puede rechazar un caso resuelto o cerrado.', 1;

            IF @Comentario IS NULL OR LTRIM(RTRIM(@Comentario)) = ''
                THROW 50022, 'Para rechazar un caso debe especificar un motivo en @Comentario.', 1;
        END

        -- 7) Actualizar caso con fechas automaticas
        UPDATE soporte.Caso
        SET
            IdEstadoCaso = @IdEstadoNuevo,

            FechaAceptacion = CASE
                WHEN @NombreEstadoNuevo = 'Asignado' AND FechaAceptacion IS NULL THEN SYSUTCDATETIME()
                ELSE FechaAceptacion
            END,

            FechaResolucion = CASE
                WHEN @NombreEstadoNuevo = 'Resuelto' AND FechaResolucion IS NULL THEN SYSUTCDATETIME()
                ELSE FechaResolucion
            END,

            FechaCierre = CASE
                WHEN @NombreEstadoNuevo IN ('Cerrado', 'Rechazado') AND FechaCierre IS NULL THEN SYSUTCDATETIME()
                ELSE FechaCierre
            END,

            FechaActualizacion = SYSUTCDATETIME()
        WHERE IdCaso = @IdCaso;

        -- 8) Trazabilidad
        SET @ComentarioFinal = COALESCE(
            @Comentario,
            'Cambio de estado: ' + COALESCE(@NombreEstadoActual,'(desconocido)') + ' -> ' + @NombreEstadoNuevo
        );

        SET @TipoEvento =
            CASE
                WHEN @NombreEstadoNuevo = 'Resuelto'   THEN 'CasoResuelto'
                WHEN @NombreEstadoNuevo = 'Cerrado'    THEN 'CasoCerrado'
                WHEN @NombreEstadoNuevo = 'Rechazado'  THEN 'CasoRechazado'
                ELSE 'EstadoCambiado'
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
            @TipoEvento,
            @ComentarioFinal,
            @IdEstadoNuevo,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        COMMIT TRANSACTION;

        -- 9) Retornar caso actualizado
        SELECT
            c.IdCaso,
            c.NumeroCaso,
            c.Descripcion,
            c.IdEstadoCaso,
            ec.NombreEstadoCaso,
            c.FechaAceptacion,
            c.FechaResolucion,
            c.FechaCierre,
            c.FechaActualizacion,
            c.IdTecnicoAsignado,
            u.NombreCompleto AS NombreTecnicoAsignado
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