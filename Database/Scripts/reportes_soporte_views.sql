CREATE OR ALTER VIEW soporte.vwReporteCasosBase
AS
WITH EncuestasCaso AS (
    SELECT
        e.IdCaso,
        AVG(CAST(r.ValorNumerico AS DECIMAL(10,2))) AS PromedioEncuesta
    FROM soporte.EncuestaCalidad e
    INNER JOIN soporte.DetalleEncuesta de ON de.IdEncuesta = e.IdEncuesta
    INNER JOIN catalogo.Respuesta r ON r.IdRespuesta = de.IdRespuesta
    WHERE r.ValorNumerico IS NOT NULL
    GROUP BY e.IdCaso
),
UltimoEscalamiento AS (
    SELECT
        tc.IdCaso,
        tc.Comentario,
        tc.FechaEvento,
        ROW_NUMBER() OVER (
            PARTITION BY tc.IdCaso
            ORDER BY tc.FechaEvento DESC, tc.IdTrazabilidadCaso DESC
        ) AS rn
    FROM soporte.TrazabilidadCaso tc
    WHERE tc.TipoEvento IN ('CasoEscalado', 'Escalado')
)
SELECT
    c.IdCaso,
    c.IdUsuarioReporta,
    c.IdTecnicoAsignado,
    c.IdEstadoCaso,
    c.IdTipoCaso,
    c.IdAreaTecnica,
    c.IdPrioridad,
    c.IdActivo,
    c.NumeroCaso,
    c.Descripcion,
    c.FechaRegistro,
    c.FechaResolucion,
    c.FechaCierre,
    COALESCE(c.FechaResolucion, c.FechaCierre) AS FechaFinCaso,
    ec.NombreEstadoCaso,
    tc.NombreTipoCaso,
    p.NombrePrioridad,
    COALESCE(NULLIF(LTRIM(RTRIM(ur.NombreCompleto)), ''), CONCAT('Usuario ', c.IdUsuarioReporta)) AS NombreUsuarioReporta,
    COALESCE(NULLIF(LTRIM(RTRIM(c.CorreoContacto)), ''), NULLIF(LTRIM(RTRIM(ur.Email)), '')) AS CorreoContacto,
    COALESCE(NULLIF(LTRIM(RTRIM(c.TelefonoContacto)), ''), NULLIF(LTRIM(RTRIM(ur.Telefono)), '')) AS TelefonoContacto,
    COALESCE(NULLIF(LTRIM(RTRIM(ta.NombreCompleto)), ''), 'Sin asignar') AS NombreTecnicoAsignado,
    COALESCE(NULLIF(LTRIM(RTRIM(ta.Email)), ''), NULLIF(LTRIM(RTRIM(ta.NombreCompleto)), '')) AS CorreoTecnicoAsignado,
    at.NombreAreaTecnica,
    p.TiempoRespuestaDias,
    p.TiempoResolucionDias,
    ci.NombreCanal,
    COALESCE(NULLIF(LTRIM(RTRIM(a.NombreActivo)), ''), 'No registrado') AS NombreActivo,
    COALESCE(NULLIF(LTRIM(RTRIM(a.CodigoPatrimonial)), ''), '') AS CodigoPatrimonial,
    COALESCE(NULLIF(LTRIM(RTRIM(a.Marca)), ''), '') AS Marca,
    COALESCE(NULLIF(LTRIM(RTRIM(a.Modelo)), ''), '') AS Modelo,
    COALESCE(NULLIF(LTRIM(RTRIM(a.Serie)), ''), '') AS Serie,
    COALESCE(NULLIF(LTRIM(RTRIM(cat.NombreCategoria)), ''), '') AS NombreCategoria,
    COALESCE(
        NULLIF(LTRIM(RTRIM(ub.Descripcion)), ''),
        NULLIF(LTRIM(RTRIM(CONCAT_WS(' - ',
            NULLIF(LTRIM(RTRIM(sd.NombreSede)), ''),
            NULLIF(LTRIM(RTRIM(ub.Bloque)), ''),
            NULLIF(LTRIM(RTRIM(ub.Piso)), ''),
            NULLIF(LTRIM(RTRIM(ub.Sala)), '')
        ))), ''),
        'No registrada'
    ) AS UbicacionTexto,
    CAST(DATEDIFF(MINUTE, c.FechaRegistro, COALESCE(c.FechaResolucion, c.FechaCierre, GETDATE())) / 60.0 AS DECIMAL(10,2)) AS HorasTranscurridas,
    CAST(CASE
        WHEN p.TiempoResolucionDias IS NULL THEN NULL
        ELSE p.TiempoResolucionDias * 24.0
    END AS DECIMAL(10,2)) AS HorasObjetivoResolucion,
    enc.PromedioEncuesta,
    CAST(CASE
        WHEN ec.NombreEstadoCaso = 'Escalado'
            OR EXISTS (
                SELECT 1
                FROM soporte.TrazabilidadCaso tr
                WHERE tr.IdCaso = c.IdCaso
                    AND tr.TipoEvento IN ('CasoEscalado', 'Escalado')
            )
        THEN 1
        ELSE 0
    END AS BIT) AS FueEscalado,
    ue.Comentario AS MotivoEscalado
FROM soporte.Caso c
INNER JOIN acceso.Usuario ur ON ur.IdUsuario = c.IdUsuarioReporta
INNER JOIN catalogo.EstadoCaso ec ON ec.IdEstadoCaso = c.IdEstadoCaso
INNER JOIN catalogo.TipoCaso tc ON tc.IdTipoCaso = c.IdTipoCaso
INNER JOIN catalogo.Prioridad p ON p.IdPrioridad = c.IdPrioridad
INNER JOIN catalogo.CanalIngreso ci ON ci.IdCanalIngreso = c.IdCanalIngreso
LEFT JOIN acceso.Usuario ta ON ta.IdUsuario = c.IdTecnicoAsignado
LEFT JOIN catalogo.AreaTecnica at ON at.IdAreaTecnica = c.IdAreaTecnica
LEFT JOIN inventario.Activo a ON a.IdActivo = c.IdActivo
LEFT JOIN catalogo.CategoriaActivo cat ON cat.IdCategoriaActivo = a.IdCategoriaActivo
LEFT JOIN inventario.Ubicacion ub ON ub.IdUbicacion = a.IdUbicacion
LEFT JOIN catalogo.Sede sd ON sd.IdSede = ub.IdSede
LEFT JOIN EncuestasCaso enc ON enc.IdCaso = c.IdCaso
LEFT JOIN UltimoEscalamiento ue ON ue.IdCaso = c.IdCaso AND ue.rn = 1;
GO

CREATE OR ALTER VIEW soporte.vwReporteEncuestasBase
AS
SELECT
    e.IdEncuesta,
    e.IdCaso,
    c.NumeroCaso,
    e.FechaEncuesta,
    e.Observaciones,
    c.NombreUsuarioReporta,
    c.NombreTecnicoAsignado,
    c.NombreAreaTecnica,
    c.NombreTipoCaso,
    c.NombrePrioridad,
    CAST(CASE
        WHEN c.FechaFinCaso IS NOT NULL THEN c.HorasTranscurridas
        ELSE 0
    END AS DECIMAL(10,2)) AS TiempoResolucionHoras,
    CAST(AVG(CAST(r.ValorNumerico AS DECIMAL(10,2))) OVER (PARTITION BY e.IdEncuesta) AS DECIMAL(10,2)) AS PromedioEncuesta,
    de.IdDetalleEncuesta,
    pr.TextoPregunta,
    r.TextoRespuesta,
    CAST(ISNULL(r.ValorNumerico, 0) AS DECIMAL(10,2)) AS ValorNumerico
FROM soporte.EncuestaCalidad e
INNER JOIN soporte.vwReporteCasosBase c ON c.IdCaso = e.IdCaso
LEFT JOIN soporte.DetalleEncuesta de ON de.IdEncuesta = e.IdEncuesta
LEFT JOIN catalogo.Pregunta pr ON pr.IdPregunta = de.IdPregunta
LEFT JOIN catalogo.Respuesta r ON r.IdRespuesta = de.IdRespuesta;
GO
