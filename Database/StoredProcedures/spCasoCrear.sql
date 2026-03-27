CREATE OR ALTER PROCEDURE soporte.spCasoCrear
    @Descripcion VARCHAR(MAX),
    @IdUsuarioReporta BIGINT,
    @TelefonoContacto VARCHAR(20) = NULL,
    @CorreoContacto VARCHAR(150) = NULL,
    @IdTipoCaso BIGINT,
    @IdPrioridad BIGINT,
    @IdCanalIngreso BIGINT,
    @IdAreaTecnica BIGINT = NULL,
    @IdActivo BIGINT = NULL,
    @IdTecnicoAsignado BIGINT = NULL,
    @IdUsuarioCreacion BIGINT,
    @IdCasoNuevo BIGINT OUTPUT,
    @NumeroCaso VARCHAR(50) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdEstadoAbierto BIGINT;
        DECLARE @IdEstadoAsignado BIGINT = NULL;
        DECLARE @NombreTecnico VARCHAR(150) = NULL;
        DECLARE @IdEstadoFinal BIGINT;

        -- 1) Estado inicial = Abierto
        SELECT @IdEstadoAbierto = IdEstadoCaso
        FROM catalogo.EstadoCaso
        WHERE NombreEstadoCaso = 'Abierto';

        IF @IdEstadoAbierto IS NULL
            THROW 50001, 'No existe el estado Abierto en catalogo.EstadoCaso.', 1;

        -- 2) Si viene tecnico asignado, validar tecnico + estado final = Asignado
        IF @IdTecnicoAsignado IS NOT NULL
        BEGIN
            -- 2.1) Validar rol del tecnico
            IF NOT EXISTS (
                SELECT 1
                FROM acceso.Usuario u
                INNER JOIN acceso.Rol r ON u.IdRol = r.IdRol
                WHERE u.IdUsuario = @IdTecnicoAsignado
                  AND r.NombreRol IN ('Tecnico', 'Administrador')
            )
            BEGIN
                THROW 50002, 'El usuario asignado no tiene rol de Tecnico o Administrador.', 1;
            END

            -- 2.2) Estado asignado
            SELECT @IdEstadoAsignado = IdEstadoCaso
            FROM catalogo.EstadoCaso
            WHERE NombreEstadoCaso = 'Asignado';

            IF @IdEstadoAsignado IS NULL
                THROW 50003, 'No existe el estado Asignado en catalogo.EstadoCaso.', 1;

            -- 2.3) Nombre del tecnico
            SELECT @NombreTecnico = NombreCompleto
            FROM acceso.Usuario
            WHERE IdUsuario = @IdTecnicoAsignado;

            IF @NombreTecnico IS NULL
                THROW 50004, 'El tecnico asignado no existe en acceso.Usuario.', 1;
        END

        SET @IdEstadoFinal = COALESCE(@IdEstadoAsignado, @IdEstadoAbierto);

        -- 3) Insertar caso
        INSERT INTO soporte.Caso (
            Descripcion,
            IdUsuarioReporta,
            TelefonoContacto,
            CorreoContacto,
            IdEstadoCaso,
            FechaAceptacion,
            IdTipoCaso,
            IdActivo,
            IdAreaTecnica,
            IdPrioridad,
            IdCanalIngreso,
            IdTecnicoAsignado,
            IdUsuarioCreacion,
            FechaActualizacion
        )
        VALUES (
            @Descripcion,
            @IdUsuarioReporta,
            @TelefonoContacto,
            @CorreoContacto,
            @IdEstadoFinal,
            CASE WHEN @IdTecnicoAsignado IS NOT NULL THEN SYSUTCDATETIME() ELSE NULL END,
            @IdTipoCaso,
            @IdActivo,
            @IdAreaTecnica,
            @IdPrioridad,
            @IdCanalIngreso,
            @IdTecnicoAsignado,
            @IdUsuarioCreacion,
            SYSUTCDATETIME()
        );

        SET @IdCasoNuevo = SCOPE_IDENTITY();

        -- 4) Leer NumeroCaso (columna calculada PERSISTED)
        SELECT @NumeroCaso = NumeroCaso
        FROM soporte.Caso
        WHERE IdCaso = @IdCasoNuevo;

        -- 5) Trazabilidad: CasoCreado
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
            @IdCasoNuevo,
            SYSUTCDATETIME(),
            @IdUsuarioCreacion,
            'CasoCreado',
            'Caso creado',
            @IdEstadoFinal,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        -- 6) Si hay tecnico desde el inicio, registrar asignacion
        IF @IdTecnicoAsignado IS NOT NULL
        BEGIN
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
                @IdCasoNuevo,
                SYSUTCDATETIME(),
                @IdUsuarioCreacion,
                'TecnicoAsignado',
                'Asignado a tecnico: ' + @NombreTecnico,
                @IdEstadoAsignado,
                @IdAreaTecnica,
                @IdTecnicoAsignado
            );
        END

        -- 7) Hoja de vida del activo: hito macro CasoCreado
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
                'Caso asociado al activo. Numero: ' + COALESCE(@NumeroCaso, '(sin_numero)'),
                'CasoCreado',
                @IdCasoNuevo,
                @IdUsuarioCreacion
            );
        END

        COMMIT TRANSACTION;

        -- 8) Retornar caso creado
        SELECT
            c.IdCaso,
            c.NumeroCaso,
            c.Descripcion,
            c.IdUsuarioReporta,
            ur.NombreCompleto AS NombreUsuarioReporta,
            c.IdEstadoCaso,
            ec.NombreEstadoCaso,
            c.IdTecnicoAsignado,
            ta.NombreCompleto AS NombreTecnicoAsignado,
            c.IdPrioridad,
            p.NombrePrioridad,
            c.IdTipoCaso,
            tc.NombreTipoCaso,
            c.IdCanalIngreso,
            ci.NombreCanal,
            c.IdAreaTecnica,
            atc.NombreAreaTecnica,
            c.FechaRegistro,
            c.FechaAceptacion,
            c.FechaActualizacion
        FROM soporte.Caso c
        INNER JOIN acceso.Usuario ur ON c.IdUsuarioReporta = ur.IdUsuario
        INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
        INNER JOIN catalogo.Prioridad p ON c.IdPrioridad = p.IdPrioridad
        INNER JOIN catalogo.TipoCaso tc ON c.IdTipoCaso = tc.IdTipoCaso
        INNER JOIN catalogo.CanalIngreso ci ON c.IdCanalIngreso = ci.IdCanalIngreso
        LEFT JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
        LEFT JOIN acceso.Usuario ta ON c.IdTecnicoAsignado = ta.IdUsuario
        WHERE c.IdCaso = @IdCasoNuevo;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

