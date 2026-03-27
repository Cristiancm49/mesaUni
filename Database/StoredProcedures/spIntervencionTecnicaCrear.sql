-- =============================================
-- Stored Procedure: spIntervencionTecnicaCrear
-- Descripción: Crear intervención técnica con componentes, consumibles y trazabilidad automática
-- =============================================

CREATE OR ALTER PROCEDURE soporte.spIntervencionTecnicaCrear
    @IdCaso BIGINT,
    @IdTipoTrabajo BIGINT,
    @IdEstadoIntervencion BIGINT,
    @FechaInicio DATETIME,
    @FechaFin DATETIME = NULL,
    @Diagnostico VARCHAR(MAX),
    @SolucionAplicada VARCHAR(MAX) = NULL,
    @IdUsuarioAccion BIGINT,
    @ComponentesJSON VARCHAR(MAX) = NULL,  -- [{"IdComponente":5,"Cantidad":2,"TipoCambio":"INSTALACION","Descripcion":"Memoria RAM"}]
    @ConsumiblesJSON VARCHAR(MAX) = NULL,  -- [{"IdConsumible":3,"Cantidad":1,"DescripcionUso":"Limpieza"}]
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

        -- 1) Validar que el caso existe
        IF NOT EXISTS (SELECT 1 FROM soporte.Caso WHERE IdCaso = @IdCaso)
            THROW 50001, 'El caso no existe.', 1;

        -- 2) Obtener información del caso
        SELECT 
            @IdTecnicoAsignado = IdTecnicoAsignado,
            @IdActivo = IdActivo,
            @IdAreaTecnica = IdAreaTecnica,
            @NumeroCaso = NumeroCaso,
            @IdEstadoCaso = IdEstadoCaso
        FROM soporte.Caso
        WHERE IdCaso = @IdCaso;

        -- 3) Validar que el caso no esté cerrado o rechazado
        SELECT @NombreEstadoCaso = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCaso;

        IF @NombreEstadoCaso IN ('Cerrado', 'Rechazado')
            THROW 50007, 'No se puede crear intervención en un caso cerrado o rechazado.', 1;

        -- 4) Validar que el caso tenga técnico asignado
        IF @IdTecnicoAsignado IS NULL
            THROW 50008, 'El caso no tiene técnico asignado.', 1;

        -- 5) Validar que solo el técnico asignado puede crear intervención
        IF @IdUsuarioAccion <> @IdTecnicoAsignado
            THROW 50002, 'Solo el técnico asignado puede crear intervención.', 1;

        -- 6) Crear registro de trazabilidad para la intervención
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
            'IntervencionRegistrada',
            'Intervencion tecnica registrada: ' + LEFT(@Diagnostico, 100),
            @IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        );

        SET @IdTrazabilidadCasoNueva = SCOPE_IDENTITY();

        -- 7) Insertar intervención técnica
        INSERT INTO soporte.IntervencionTecnica (
            IdTrazabilidadCaso,
            IdTipoTrabajo,
            IdEstadoIntervencion,
            FechaInicio,
            FechaFin,
            Diagnostico,
            SolucionAplicada,
            IdUsuarioAccion
        )
        VALUES (
            @IdTrazabilidadCasoNueva,
            @IdTipoTrabajo,
            @IdEstadoIntervencion,
            @FechaInicio,
            @FechaFin,
            @Diagnostico,
            @SolucionAplicada,
            @IdUsuarioAccion
        );

        SET @IdIntervencionNueva = SCOPE_IDENTITY();

        -- 8) Procesar componentes (si hay)
        IF @ComponentesJSON IS NOT NULL AND @ComponentesJSON <> '[]'
        BEGIN
            -- Insertar detalles de componentes
            INSERT INTO soporte.DetalleCambioComponentes (
                IdIntervencionTecnica,
                IdComponente,
                Cantidad,
                TipoCambio,
                DescripcionCambio,
                IdUsuarioCreacion
            )
            SELECT 
                @IdIntervencionNueva,
                JSON_VALUE(value, '$.IdComponente'),
                JSON_VALUE(value, '$.Cantidad'),
                JSON_VALUE(value, '$.TipoCambio'),
                JSON_VALUE(value, '$.Descripcion'),
                @IdUsuarioAccion
            FROM OPENJSON(@ComponentesJSON);

            -- Actualizar stock de componentes
            UPDATE inventario.Componente
            SET StockActual = StockActual - JSON_VALUE(j.value, '$.Cantidad')
            FROM OPENJSON(@ComponentesJSON) j
            WHERE Componente.IdComponente = JSON_VALUE(j.value, '$.IdComponente')
            AND JSON_VALUE(j.value, '$.TipoCambio') IN ('INSTALACION', 'REEMPLAZO');

            -- Devolver stock si es RETIRO
            UPDATE inventario.Componente
            SET StockActual = StockActual + JSON_VALUE(j.value, '$.Cantidad')
            FROM OPENJSON(@ComponentesJSON) j
            WHERE Componente.IdComponente = JSON_VALUE(j.value, '$.IdComponente')
            AND JSON_VALUE(j.value, '$.TipoCambio') = 'RETIRO';

            -- Validar que hay stock suficiente
            IF EXISTS (
                SELECT 1
                FROM inventario.Componente c
                INNER JOIN OPENJSON(@ComponentesJSON) j ON c.IdComponente = JSON_VALUE(j.value, '$.IdComponente')
                WHERE c.StockActual < 0
            )
            BEGIN
                DECLARE @ComponentesSinStock VARCHAR(500);
                SELECT @ComponentesSinStock = STRING_AGG(c.NombreComponente, ', ')
                FROM inventario.Componente c
                WHERE c.StockActual < 0;
                
                DECLARE @MensajeError VARCHAR(600) = 'Stock insuficiente para componentes: ' + COALESCE(@ComponentesSinStock, 'N/A');
                THROW 50003, @MensajeError, 1;
            END
        END

        -- 9) Procesar consumibles (si hay)
        IF @ConsumiblesJSON IS NOT NULL AND @ConsumiblesJSON <> '[]'
        BEGIN
            -- Insertar detalles de consumibles
            INSERT INTO soporte.DetalleConsumible (
                IdIntervencionTecnica,
                IdConsumible,
                Cantidad,
                DescripcionUso,
                IdUsuarioCreacion
            )
            SELECT 
                @IdIntervencionNueva,
                JSON_VALUE(value, '$.IdConsumible'),
                JSON_VALUE(value, '$.Cantidad'),
                JSON_VALUE(value, '$.DescripcionUso'),
                @IdUsuarioAccion
            FROM OPENJSON(@ConsumiblesJSON);

            -- Descontar stock de consumibles
            UPDATE inventario.Consumible
            SET StockActual = StockActual - JSON_VALUE(j.value, '$.Cantidad')
            FROM OPENJSON(@ConsumiblesJSON) j
            WHERE Consumible.IdConsumible = JSON_VALUE(j.value, '$.IdConsumible');

            -- Validar stock de consumibles
            IF EXISTS (
                SELECT 1
                FROM inventario.Consumible c
                INNER JOIN OPENJSON(@ConsumiblesJSON) j ON c.IdConsumible = JSON_VALUE(j.value, '$.IdConsumible')
                WHERE c.StockActual < 0
            )
            BEGIN
                DECLARE @ConsumiblesSinStock VARCHAR(500);
                SELECT @ConsumiblesSinStock = STRING_AGG(c.NombreConsumible, ', ')
                FROM inventario.Consumible c
                WHERE c.StockActual < 0;
                
                DECLARE @MensajeError2 VARCHAR(600) = 'Stock insuficiente para consumibles: ' + COALESCE(@ConsumiblesSinStock, 'N/A');
                THROW 50004, @MensajeError2, 1;
            END
        END

        -- 10) Registrar en hoja de vida del activo (si existe)
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
                'Intervencion tecnica: ' + LEFT(@Diagnostico, 200),
                'Mantenimiento',
                @IdCaso,
                @IdUsuarioAccion
            );
        END

        COMMIT TRANSACTION;

        -- 11) Retornar intervención creada con detalles
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

        -- Result Set 2: Componentes usados
        SELECT
            dc.IdCambioComponente,
            dc.IdComponente,
            c.NombreComponente,
            dc.Cantidad,
            dc.TipoCambio,
            dc.DescripcionCambio
        FROM soporte.DetalleCambioComponentes dc
        INNER JOIN inventario.Componente c ON dc.IdComponente = c.IdComponente
        WHERE dc.IdIntervencionTecnica = @IdIntervencionNueva;

        -- Result Set 3: Consumibles usados
        SELECT
            dco.IdDetalleConsumible,
            dco.IdConsumible,
            co.NombreConsumible,
            dco.Cantidad,
            dco.DescripcionUso
        FROM soporte.DetalleConsumible dco
        INNER JOIN inventario.Consumible co ON dco.IdConsumible = co.IdConsumible
        WHERE dco.IdIntervencionTecnica = @IdIntervencionNueva;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
