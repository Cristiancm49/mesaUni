CREATE OR ALTER PROCEDURE soporte.spRevisionAdmiProcesar
    @IdIntervencionTecnica BIGINT,
    @Aprobado BIT,
    @TipoRevision VARCHAR(30) = 'DIAGNOSTICO',
    @ObservacionRevision VARCHAR(MAX) = NULL,
    @IdUsuarioCreacion BIGINT,
    @IdRevisionNueva BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdCaso BIGINT;
        DECLARE @IdTrazabilidadCaso BIGINT;
        DECLARE @IdEstadoAprobado BIGINT;
        DECLARE @IdEstadoCompletado BIGINT;
        DECLARE @IdEstadoRechazado BIGINT;
        DECLARE @IdEstadoPendienteAprobacion BIGINT;
        DECLARE @IdEstadoPendienteAprobacionSolucion BIGINT;
        DECLARE @IdEstadoSolucionRechazada BIGINT;
        DECLARE @IdEstadoCasoActual BIGINT;
        DECLARE @IdEstadoCasoResuelto BIGINT;
        DECLARE @IdAreaTecnica BIGINT;
        DECLARE @IdTecnicoAsignado BIGINT;
        DECLARE @TipoEvento VARCHAR(100);
        DECLARE @Comentario VARCHAR(500);
        DECLARE @TipoRevisionDetectada VARCHAR(30);
        DECLARE @NombreEstadoCasoActual VARCHAR(100);
        DECLARE @IdEstadoIntervencionActual BIGINT;
        DECLARE @NombreEstadoIntervencionActual VARCHAR(100);

        SET @TipoRevision = UPPER(LTRIM(RTRIM(COALESCE(@TipoRevision, 'DIAGNOSTICO'))));

        IF @TipoRevision NOT IN ('DIAGNOSTICO', 'SOLUCION')
            THROW 50000, 'El tipo de revision debe ser DIAGNOSTICO o SOLUCION.', 1;

        SELECT
            @IdTrazabilidadCaso = i.IdTrazabilidadCaso,
            @IdCaso = t.IdCaso,
            @IdEstadoIntervencionActual = i.IdEstadoIntervencion
        FROM soporte.IntervencionTecnica i
        INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
        WHERE i.IdIntervencionTecnica = @IdIntervencionTecnica;

        IF @IdCaso IS NULL
            THROW 50001, 'La intervencion tecnica no existe.', 1;

        SELECT @NombreEstadoIntervencionActual = eit.NombreEstado
        FROM catalogo.EstadoIntervencionTecnica eit
        WHERE eit.IdEstadoIntervencion = @IdEstadoIntervencionActual;

        SET @TipoRevisionDetectada = CASE
            WHEN @NombreEstadoIntervencionActual = 'Pendiente Aprobacion' THEN 'DIAGNOSTICO'
            WHEN @NombreEstadoIntervencionActual = 'Pendiente Aprobacion Solucion' THEN 'SOLUCION'
            ELSE NULL
        END;

        IF @TipoRevisionDetectada IS NULL
            THROW 50008, 'La intervencion no se encuentra en una fase valida de revision administrativa.', 1;

        -- El estado real de la intervencion manda sobre el payload para evitar
        -- que una aprobacion de diagnostico entre por la rama de solucion.
        SET @TipoRevision = @TipoRevisionDetectada;

        SELECT @IdEstadoAprobado = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Aprobada';

        SELECT @IdEstadoRechazado = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Rechazada';

        SELECT @IdEstadoCompletado = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Completado';

        SELECT @IdEstadoPendienteAprobacion = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Pendiente Aprobacion';

        SELECT @IdEstadoPendienteAprobacionSolucion = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Pendiente Aprobacion Solucion';

        SELECT @IdEstadoSolucionRechazada = IdEstadoIntervencion
        FROM catalogo.EstadoIntervencionTecnica
        WHERE NombreEstado = 'Solucion Rechazada';

        IF @IdEstadoAprobado IS NULL OR @IdEstadoCompletado IS NULL OR @IdEstadoRechazado IS NULL OR @IdEstadoPendienteAprobacion IS NULL
            THROW 50002, 'No se encontraron los estados requeridos para revisar diagnosticos.', 1;

        IF @IdEstadoPendienteAprobacionSolucion IS NULL OR @IdEstadoSolucionRechazada IS NULL
            THROW 50003, 'No se encontraron los estados requeridos para revisar soluciones.', 1;

        IF @TipoRevision = 'DIAGNOSTICO' AND @NombreEstadoIntervencionActual <> 'Pendiente Aprobacion'
            THROW 50004, 'La intervencion no esta en estado "Pendiente Aprobacion".', 1;

        IF @TipoRevision = 'SOLUCION' AND @NombreEstadoIntervencionActual <> 'Pendiente Aprobacion Solucion'
            THROW 50005, 'La intervencion no esta en estado "Pendiente Aprobacion Solucion".', 1;

        SELECT
            @IdAreaTecnica = c.IdAreaTecnica,
            @IdTecnicoAsignado = c.IdTecnicoAsignado,
            @IdEstadoCasoActual = c.IdEstadoCaso
        FROM soporte.Caso c
        WHERE c.IdCaso = @IdCaso;

        SELECT @NombreEstadoCasoActual = ec.NombreEstadoCaso
        FROM catalogo.EstadoCaso ec
        WHERE ec.IdEstadoCaso = @IdEstadoCasoActual;

        IF @NombreEstadoCasoActual IN ('Cerrado', 'Rechazado')
            THROW 50006, 'No se puede procesar revision de un caso cerrado o rechazado.', 1;

        INSERT INTO soporte.RevisionAdmi (
            IdIntervencionTecnica,
            Aprobado,
            TipoRevision,
            ObservacionRevision,
            IdUsuarioCreacion
        )
        VALUES (
            @IdIntervencionTecnica,
            @Aprobado,
            @TipoRevision,
            @ObservacionRevision,
            @IdUsuarioCreacion
        );

        SET @IdRevisionNueva = SCOPE_IDENTITY();

        IF @TipoRevision = 'DIAGNOSTICO'
        BEGIN
            IF @Aprobado = 1
            BEGIN
                UPDATE soporte.IntervencionTecnica
                SET IdEstadoIntervencion = @IdEstadoAprobado
                WHERE IdIntervencionTecnica = @IdIntervencionTecnica;

                SET @TipoEvento = 'DiagnosticoAprobado';
                SET @Comentario = 'Diagnostico aprobado administrativamente. La intervencion queda lista para ejecutar. ' + COALESCE(@ObservacionRevision, '');
            END
            ELSE
            BEGIN
                UPDATE soporte.IntervencionTecnica
                SET IdEstadoIntervencion = @IdEstadoRechazado
                WHERE IdIntervencionTecnica = @IdIntervencionTecnica;

                SET @TipoEvento = 'DiagnosticoRechazado';
                SET @Comentario = 'Diagnostico rechazado administrativamente. El tecnico debe corregirlo y reenviarlo. ' + COALESCE(@ObservacionRevision, '');
            END
        END
        ELSE
        BEGIN
            SELECT @IdEstadoCasoResuelto = IdEstadoCaso
            FROM catalogo.EstadoCaso
            WHERE NombreEstadoCaso = 'Resuelto';

            IF @IdEstadoCasoResuelto IS NULL
                THROW 50007, 'No existe el estado de caso "Resuelto".', 1;

            IF @Aprobado = 1
            BEGIN
                UPDATE soporte.IntervencionTecnica
                SET IdEstadoIntervencion = @IdEstadoCompletado
                WHERE IdIntervencionTecnica = @IdIntervencionTecnica;

                UPDATE soporte.Caso
                SET
                    IdEstadoCaso = @IdEstadoCasoResuelto,
                    FechaResolucion = COALESCE(FechaResolucion, SYSUTCDATETIME()),
                    FechaActualizacion = SYSUTCDATETIME()
                WHERE IdCaso = @IdCaso;

                SET @TipoEvento = 'SolucionAprobada';
                SET @Comentario = 'Solucion aprobada administrativamente. El caso queda resuelto. ' + COALESCE(@ObservacionRevision, '');
            END
            ELSE
            BEGIN
                UPDATE soporte.IntervencionTecnica
                SET IdEstadoIntervencion = @IdEstadoSolucionRechazada
                WHERE IdIntervencionTecnica = @IdIntervencionTecnica;

                UPDATE soporte.Caso
                SET FechaActualizacion = SYSUTCDATETIME()
                WHERE IdCaso = @IdCaso;

                SET @TipoEvento = 'SolucionRechazada';
                SET @Comentario = 'Solucion rechazada administrativamente. El tecnico debe corregirla y reenviarla. ' + COALESCE(@ObservacionRevision, '');
            END
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
            @IdUsuarioCreacion,
            @TipoEvento,
            @Comentario,
            c.IdEstadoCaso,
            @IdAreaTecnica,
            @IdTecnicoAsignado
        FROM soporte.Caso c
        WHERE c.IdCaso = @IdCaso;

        COMMIT TRANSACTION;

        SELECT
            r.IdRevisionAdmi,
            r.IdIntervencionTecnica,
            r.Aprobado,
            r.TipoRevision,
            r.ObservacionRevision,
            r.FechaRegistro,
            r.IdUsuarioCreacion,
            u.NombreCompleto AS NombreRevisor,
            t.IdCaso,
            c.NumeroCaso
        FROM soporte.RevisionAdmi r
        INNER JOIN acceso.Usuario u ON r.IdUsuarioCreacion = u.IdUsuario
        INNER JOIN soporte.IntervencionTecnica i ON r.IdIntervencionTecnica = i.IdIntervencionTecnica
        INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
        INNER JOIN soporte.Caso c ON t.IdCaso = c.IdCaso
        WHERE r.IdRevisionAdmi = @IdRevisionNueva;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
