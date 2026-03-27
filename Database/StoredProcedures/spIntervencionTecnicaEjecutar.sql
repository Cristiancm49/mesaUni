CREATE OR ALTER PROCEDURE soporte.spIntervencionTecnicaEjecutar
    @IdIntervencionTecnica BIGINT,
    @SolucionAplicada VARCHAR(MAX),
    @IdUsuarioAccion BIGINT,
    @ComponentesJSON VARCHAR(MAX) = NULL,
    @ConsumiblesJSON VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdCaso BIGINT;
        DECLARE @IdTrazabilidadCaso BIGINT;
        DECLARE @IdEstadoPendienteAprobacionSolucion BIGINT;
        DECLARE @IdEstadoCasoActual BIGINT;
        DECLARE @NombreEstadoCasoActual VARCHAR(100);
        DECLARE @IdEstadoIntervencionActual BIGINT;
        DECLARE @NombreEstadoIntervencionActual VARCHAR(100);
        DECLARE @IdActivo BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @IdTecnicoAsignado BIGINT;

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

        SELECT @NombreEstadoIntervencionActual = NombreEstado
        FROM catalogo.EstadoIntervencionTecnica
        WHERE IdEstadoIntervencion = @IdEstadoIntervencionActual;

        IF @NombreEstadoIntervencionActual <> 'Aprobada'
            THROW 50002, 'La intervencion tecnica debe estar en estado "Aprobada" para poder ejecutar la solucion.', 1;

        SELECT
            @IdTecnicoAsignado = c.IdTecnicoAsignado,
            @IdActivo = c.IdActivo,
            @IdAreaTecnica = c.IdAreaTecnica,
            @IdEstadoCasoActual = c.IdEstadoCaso
        FROM soporte.Caso c
        WHERE c.IdCaso = @IdCaso;

        IF @IdTecnicoAsignado IS NULL OR @IdUsuarioAccion <> @IdTecnicoAsignado
            THROW 50003, 'Solo el tecnico asignado al caso puede ejecutar la intervencion.', 1;

        SELECT @NombreEstadoCasoActual = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCasoActual;

        IF @NombreEstadoCasoActual IN ('Cerrado', 'Rechazado')
            THROW 50004, 'No se puede ejecutar una intervencion sobre un caso cerrado o rechazado.', 1;

        SELECT @IdEstadoPendienteAprobacionSolucion = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Pendiente Aprobacion Solucion';

        IF @IdEstadoPendienteAprobacionSolucion IS NULL
            THROW 50005, 'No existe el estado "Pendiente Aprobacion Solucion" en catalogo.EstadoIntervencionTecnica.', 1;

        UPDATE soporte.IntervencionTecnica
        SET
            SolucionAplicada = @SolucionAplicada,
            FechaFin = SYSUTCDATETIME(),
            IdEstadoIntervencion = @IdEstadoPendienteAprobacionSolucion
        WHERE IdIntervencionTecnica = @IdIntervencionTecnica;

        IF @ComponentesJSON IS NOT NULL AND @ComponentesJSON <> '[]'
        BEGIN
            INSERT INTO soporte.DetalleCambioComponentes (
                IdIntervencionTecnica,
                IdComponente,
                Cantidad,
                TipoCambio,
                DescripcionCambio,
                FechaRegistro,
                IdUsuarioCreacion
            )
            SELECT
                @IdIntervencionTecnica,
                JSON_VALUE(value, '$.IdComponente'),
                JSON_VALUE(value, '$.Cantidad'),
                JSON_VALUE(value, '$.TipoCambio'),
                JSON_VALUE(value, '$.DescripcionCambio'),
                SYSUTCDATETIME(),
                @IdUsuarioAccion
            FROM OPENJSON(@ComponentesJSON);

            UPDATE inventario.Componente
            SET StockActual = StockActual - CAST(JSON_VALUE(j.value, '$.Cantidad') AS INT)
            FROM OPENJSON(@ComponentesJSON) j
            WHERE Componente.IdComponente = CAST(JSON_VALUE(j.value, '$.IdComponente') AS BIGINT)
              AND JSON_VALUE(j.value, '$.TipoCambio') IN ('INSTALACION', 'REEMPLAZO');

            UPDATE inventario.Componente
            SET StockActual = StockActual + CAST(JSON_VALUE(j.value, '$.Cantidad') AS INT)
            FROM OPENJSON(@ComponentesJSON) j
            WHERE Componente.IdComponente = CAST(JSON_VALUE(j.value, '$.IdComponente') AS BIGINT)
              AND JSON_VALUE(j.value, '$.TipoCambio') = 'RETIRO';

            IF EXISTS (
                SELECT 1
                FROM inventario.Componente c
                INNER JOIN OPENJSON(@ComponentesJSON) j
                    ON c.IdComponente = CAST(JSON_VALUE(j.value, '$.IdComponente') AS BIGINT)
                WHERE c.StockActual < 0
            )
            BEGIN
                DECLARE @ComponentesSinStock VARCHAR(500);

                SELECT @ComponentesSinStock = STRING_AGG(c.NombreComponente, ', ')
                FROM inventario.Componente c
                WHERE c.StockActual < 0;

                DECLARE @MensajeError VARCHAR(600) = 'Stock insuficiente para componentes: ' + COALESCE(@ComponentesSinStock, 'N/A');
                THROW 50006, @MensajeError, 1;
            END
        END

        IF @ConsumiblesJSON IS NOT NULL AND @ConsumiblesJSON <> '[]'
        BEGIN
            INSERT INTO soporte.DetalleConsumible (
                IdIntervencionTecnica,
                IdConsumible,
                Cantidad,
                DescripcionUso,
                FechaRegistro,
                IdUsuarioCreacion
            )
            SELECT
                @IdIntervencionTecnica,
                JSON_VALUE(value, '$.IdConsumible'),
                JSON_VALUE(value, '$.Cantidad'),
                JSON_VALUE(value, '$.DescripcionUso'),
                SYSUTCDATETIME(),
                @IdUsuarioAccion
            FROM OPENJSON(@ConsumiblesJSON);

            UPDATE inventario.Consumible
            SET StockActual = StockActual - CAST(JSON_VALUE(j.value, '$.Cantidad') AS INT)
            FROM OPENJSON(@ConsumiblesJSON) j
            WHERE Consumible.IdConsumible = CAST(JSON_VALUE(j.value, '$.IdConsumible') AS BIGINT);

            IF EXISTS (
                SELECT 1
                FROM inventario.Consumible c
                INNER JOIN OPENJSON(@ConsumiblesJSON) j
                    ON c.IdConsumible = CAST(JSON_VALUE(j.value, '$.IdConsumible') AS BIGINT)
                WHERE c.StockActual < 0
            )
            BEGIN
                DECLARE @ConsumiblesSinStock VARCHAR(500);

                SELECT @ConsumiblesSinStock = STRING_AGG(c.NombreConsumible, ', ')
                FROM inventario.Consumible c
                WHERE c.StockActual < 0;

                DECLARE @MensajeError2 VARCHAR(600) = 'Stock insuficiente para consumibles: ' + COALESCE(@ConsumiblesSinStock, 'N/A');
                THROW 50007, @MensajeError2, 1;
            END
        END

        UPDATE soporte.Caso
        SET FechaActualizacion = SYSUTCDATETIME()
        WHERE IdCaso = @IdCaso;

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
                'Solucion registrada para revision final: ' + LEFT(@SolucionAplicada, 200),
                'Mantenimiento',
                @IdCaso,
                @IdUsuarioAccion
            );
        END

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
        SELECT
            @IdCaso,
            SYSUTCDATETIME(),
            @IdUsuarioAccion,
            'SolucionRegistrada',
            'Solucion registrada y enviada a revision administrativa final: ' + LEFT(@SolucionAplicada, 100),
            c.IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        FROM soporte.Caso c
        WHERE c.IdCaso = @IdCaso;

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

        SELECT
            dc.IdCambioComponente,
            dc.IdComponente,
            comp.NombreComponente,
            dc.Cantidad,
            dc.TipoCambio,
            dc.DescripcionCambio
        FROM soporte.DetalleCambioComponentes dc
        INNER JOIN inventario.Componente comp ON dc.IdComponente = comp.IdComponente
        WHERE dc.IdIntervencionTecnica = @IdIntervencionTecnica;

        SELECT
            dco.IdDetalleConsumible,
            dco.IdConsumible,
            cons.NombreConsumible,
            dco.Cantidad,
            dco.DescripcionUso
        FROM soporte.DetalleConsumible dco
        INNER JOIN inventario.Consumible cons ON dco.IdConsumible = cons.IdConsumible
        WHERE dco.IdIntervencionTecnica = @IdIntervencionTecnica;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
