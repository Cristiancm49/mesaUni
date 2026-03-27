CREATE OR ALTER PROCEDURE soporte.spRevisionAdmiObtenerBandeja
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    ;WITH Base AS (
        SELECT
            i.IdIntervencionTecnica,
            c.IdCaso,
            c.NumeroCaso,
            c.Descripcion AS DescripcionCaso,
            tt.NombreTipoTrabajo,
            eit.NombreEstado AS NombreEstadoIntervencion,
            ec.NombreEstadoCaso,
            p.NombrePrioridad,
            tec.NombreCompleto AS NombreTecnicoAsignado,
            atc.NombreAreaTecnica,
            ur.NombreCompleto AS NombreUsuarioReporta,
            a.NombreActivo,
            i.FechaInicio,
            i.FechaFin,
            i.Diagnostico,
            i.SolucionAplicada,
            CAST((SELECT COUNT(*) FROM soporte.DetalleCambioComponentes dcc WHERE dcc.IdIntervencionTecnica = i.IdIntervencionTecnica) AS BIGINT) AS CantidadComponentes,
            CAST((SELECT COUNT(*) FROM soporte.DetalleConsumible dc WHERE dc.IdIntervencionTecnica = i.IdIntervencionTecnica) AS BIGINT) AS CantidadConsumibles
        FROM soporte.IntervencionTecnica i
        INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
        INNER JOIN soporte.Caso c ON t.IdCaso = c.IdCaso
        INNER JOIN catalogo.TipoTrabajo tt ON i.IdTipoTrabajo = tt.IdTipoTrabajo
        INNER JOIN catalogo.EstadoIntervencionTecnica eit ON i.IdEstadoIntervencion = eit.IdEstadoIntervencion
        INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
        INNER JOIN catalogo.Prioridad p ON c.IdPrioridad = p.IdPrioridad
        LEFT JOIN acceso.Usuario tec ON c.IdTecnicoAsignado = tec.IdUsuario
        LEFT JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
        LEFT JOIN acceso.Usuario ur ON c.IdUsuarioReporta = ur.IdUsuario
        LEFT JOIN inventario.Activo a ON c.IdActivo = a.IdActivo
    ),
    UltimaRevision AS (
        SELECT
            ra.IdRevisionAdmi,
            ra.IdIntervencionTecnica,
            ra.Aprobado,
            ra.TipoRevision,
            ra.ObservacionRevision,
            ra.FechaRegistro,
            u.NombreCompleto AS NombreRevisor,
            ROW_NUMBER() OVER (
                PARTITION BY ra.IdIntervencionTecnica, ra.TipoRevision
                ORDER BY ra.FechaRegistro DESC, ra.IdRevisionAdmi DESC
            ) AS RowNum
        FROM soporte.RevisionAdmi ra
        INNER JOIN acceso.Usuario u ON ra.IdUsuarioCreacion = u.IdUsuario
    )
    SELECT
        b.IdIntervencionTecnica,
        b.IdCaso,
        b.NumeroCaso,
        b.DescripcionCaso,
        'DIAGNOSTICO' AS TipoRevision,
        b.NombreTipoTrabajo,
        b.NombreEstadoIntervencion,
        b.NombreEstadoCaso,
        b.NombrePrioridad,
        b.NombreTecnicoAsignado,
        b.NombreAreaTecnica,
        b.NombreUsuarioReporta,
        b.NombreActivo,
        b.FechaInicio,
        b.FechaFin,
        b.Diagnostico,
        b.SolucionAplicada,
        b.CantidadComponentes,
        b.CantidadConsumibles,
        ur.IdRevisionAdmi,
        ur.Aprobado,
        CASE
            WHEN ur.Aprobado = 1 THEN 'APROBADO'
            WHEN ur.Aprobado = 0 THEN 'RECHAZADO'
            ELSE NULL
        END AS EstadoAprobacion,
        ur.ObservacionRevision,
        ur.FechaRegistro AS FechaRevision,
        ur.NombreRevisor
    FROM Base b
    LEFT JOIN UltimaRevision ur
        ON ur.IdIntervencionTecnica = b.IdIntervencionTecnica
        AND ur.TipoRevision = 'DIAGNOSTICO'
        AND ur.RowNum = 1
    WHERE b.NombreEstadoIntervencion = 'Pendiente Aprobacion'
    ORDER BY b.FechaInicio DESC, b.IdIntervencionTecnica DESC;

    ;WITH Base AS (
        SELECT
            i.IdIntervencionTecnica,
            c.IdCaso,
            c.NumeroCaso,
            c.Descripcion AS DescripcionCaso,
            tt.NombreTipoTrabajo,
            eit.NombreEstado AS NombreEstadoIntervencion,
            ec.NombreEstadoCaso,
            p.NombrePrioridad,
            tec.NombreCompleto AS NombreTecnicoAsignado,
            atc.NombreAreaTecnica,
            ur.NombreCompleto AS NombreUsuarioReporta,
            a.NombreActivo,
            i.FechaInicio,
            i.FechaFin,
            i.Diagnostico,
            i.SolucionAplicada,
            CAST((SELECT COUNT(*) FROM soporte.DetalleCambioComponentes dcc WHERE dcc.IdIntervencionTecnica = i.IdIntervencionTecnica) AS BIGINT) AS CantidadComponentes,
            CAST((SELECT COUNT(*) FROM soporte.DetalleConsumible dc WHERE dc.IdIntervencionTecnica = i.IdIntervencionTecnica) AS BIGINT) AS CantidadConsumibles
        FROM soporte.IntervencionTecnica i
        INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
        INNER JOIN soporte.Caso c ON t.IdCaso = c.IdCaso
        INNER JOIN catalogo.TipoTrabajo tt ON i.IdTipoTrabajo = tt.IdTipoTrabajo
        INNER JOIN catalogo.EstadoIntervencionTecnica eit ON i.IdEstadoIntervencion = eit.IdEstadoIntervencion
        INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
        INNER JOIN catalogo.Prioridad p ON c.IdPrioridad = p.IdPrioridad
        LEFT JOIN acceso.Usuario tec ON c.IdTecnicoAsignado = tec.IdUsuario
        LEFT JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
        LEFT JOIN acceso.Usuario ur ON c.IdUsuarioReporta = ur.IdUsuario
        LEFT JOIN inventario.Activo a ON c.IdActivo = a.IdActivo
    ),
    UltimaRevision AS (
        SELECT
            ra.IdRevisionAdmi,
            ra.IdIntervencionTecnica,
            ra.Aprobado,
            ra.TipoRevision,
            ra.ObservacionRevision,
            ra.FechaRegistro,
            u.NombreCompleto AS NombreRevisor,
            ROW_NUMBER() OVER (
                PARTITION BY ra.IdIntervencionTecnica, ra.TipoRevision
                ORDER BY ra.FechaRegistro DESC, ra.IdRevisionAdmi DESC
            ) AS RowNum
        FROM soporte.RevisionAdmi ra
        INNER JOIN acceso.Usuario u ON ra.IdUsuarioCreacion = u.IdUsuario
    )
    SELECT
        b.IdIntervencionTecnica,
        b.IdCaso,
        b.NumeroCaso,
        b.DescripcionCaso,
        'SOLUCION' AS TipoRevision,
        b.NombreTipoTrabajo,
        b.NombreEstadoIntervencion,
        b.NombreEstadoCaso,
        b.NombrePrioridad,
        b.NombreTecnicoAsignado,
        b.NombreAreaTecnica,
        b.NombreUsuarioReporta,
        b.NombreActivo,
        b.FechaInicio,
        b.FechaFin,
        b.Diagnostico,
        b.SolucionAplicada,
        b.CantidadComponentes,
        b.CantidadConsumibles,
        ur.IdRevisionAdmi,
        ur.Aprobado,
        CASE
            WHEN ur.Aprobado = 1 THEN 'APROBADO'
            WHEN ur.Aprobado = 0 THEN 'RECHAZADO'
            ELSE NULL
        END AS EstadoAprobacion,
        ur.ObservacionRevision,
        ur.FechaRegistro AS FechaRevision,
        ur.NombreRevisor
    FROM Base b
    LEFT JOIN UltimaRevision ur
        ON ur.IdIntervencionTecnica = b.IdIntervencionTecnica
        AND ur.TipoRevision = 'SOLUCION'
        AND ur.RowNum = 1
    WHERE b.NombreEstadoIntervencion = 'Pendiente Aprobacion Solucion'
    ORDER BY COALESCE(b.FechaFin, b.FechaInicio) DESC, b.IdIntervencionTecnica DESC;

    SELECT
        i.IdIntervencionTecnica,
        c.IdCaso,
        c.NumeroCaso,
        c.Descripcion AS DescripcionCaso,
        ra.TipoRevision,
        tt.NombreTipoTrabajo,
        eit.NombreEstado AS NombreEstadoIntervencion,
        ec.NombreEstadoCaso,
        p.NombrePrioridad,
        tec.NombreCompleto AS NombreTecnicoAsignado,
        atc.NombreAreaTecnica,
        ur.NombreCompleto AS NombreUsuarioReporta,
        a.NombreActivo,
        i.FechaInicio,
        i.FechaFin,
        i.Diagnostico,
        i.SolucionAplicada,
        CAST((SELECT COUNT(*) FROM soporte.DetalleCambioComponentes dcc WHERE dcc.IdIntervencionTecnica = i.IdIntervencionTecnica) AS BIGINT) AS CantidadComponentes,
        CAST((SELECT COUNT(*) FROM soporte.DetalleConsumible dc WHERE dc.IdIntervencionTecnica = i.IdIntervencionTecnica) AS BIGINT) AS CantidadConsumibles,
        ra.IdRevisionAdmi,
        ra.Aprobado,
        CASE
            WHEN ra.Aprobado = 1 THEN 'APROBADO'
            WHEN ra.Aprobado = 0 THEN 'RECHAZADO'
            ELSE NULL
        END AS EstadoAprobacion,
        ra.ObservacionRevision,
        ra.FechaRegistro AS FechaRevision,
        revisor.NombreCompleto AS NombreRevisor
    FROM soporte.RevisionAdmi ra
    INNER JOIN soporte.IntervencionTecnica i ON ra.IdIntervencionTecnica = i.IdIntervencionTecnica
    INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
    INNER JOIN soporte.Caso c ON t.IdCaso = c.IdCaso
    INNER JOIN catalogo.TipoTrabajo tt ON i.IdTipoTrabajo = tt.IdTipoTrabajo
    INNER JOIN catalogo.EstadoIntervencionTecnica eit ON i.IdEstadoIntervencion = eit.IdEstadoIntervencion
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    INNER JOIN catalogo.Prioridad p ON c.IdPrioridad = p.IdPrioridad
    INNER JOIN acceso.Usuario revisor ON ra.IdUsuarioCreacion = revisor.IdUsuario
    LEFT JOIN acceso.Usuario tec ON c.IdTecnicoAsignado = tec.IdUsuario
    LEFT JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
    LEFT JOIN acceso.Usuario ur ON c.IdUsuarioReporta = ur.IdUsuario
    LEFT JOIN inventario.Activo a ON c.IdActivo = a.IdActivo
    ORDER BY ra.FechaRegistro DESC, ra.IdRevisionAdmi DESC;
END;
GO
