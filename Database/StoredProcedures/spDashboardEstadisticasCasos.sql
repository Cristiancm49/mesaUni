

CREATE OR ALTER PROCEDURE soporte.spDashboardEstadisticasCasos
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL,
    @IdAreaTecnica BIGINT = NULL,
    @IdTecnico BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Si no se especifican fechas, usar últimos 30 días
    IF @FechaDesde IS NULL
        SET @FechaDesde = DATEADD(DAY, -30, SYSUTCDATETIME());
    
    IF @FechaHasta IS NULL
        SET @FechaHasta = SYSUTCDATETIME();

    -- Result Set 1: Resumen general
    SELECT 
        COUNT(*) AS TotalCasos,
        COUNT(CASE WHEN ec.NombreEstadoCaso IN ('Abierto', 'Asignado', 'En Proceso', 'Escalado') THEN 1 END) AS CasosAbiertos,
        COUNT(CASE WHEN ec.NombreEstadoCaso = 'Resuelto' THEN 1 END) AS CasosResueltos,
        COUNT(CASE WHEN ec.NombreEstadoCaso = 'Cerrado' THEN 1 END) AS CasosCerrados,
        COUNT(CASE WHEN c.IdTecnicoAsignado IS NULL THEN 1 END) AS CasosSinAsignar,
        AVG(CASE 
            WHEN c.FechaResolucion IS NOT NULL 
            THEN DATEDIFF(HOUR, c.FechaRegistro, c.FechaResolucion) 
        END) AS TiempoPromedioResolucionHoras,
        AVG(CASE 
            WHEN c.FechaCierre IS NOT NULL 
            THEN DATEDIFF(HOUR, c.FechaRegistro, c.FechaCierre) 
        END) AS TiempoPromedioCierreHoras
    FROM soporte.Caso c
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico);

    -- Result Set 2: Casos por estado
    SELECT 
        ec.IdEstadoCaso,
        ec.NombreEstadoCaso,
        COUNT(*) AS Cantidad,
        CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM soporte.Caso 
            WHERE FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
            AND (@IdAreaTecnica IS NULL OR IdAreaTecnica = @IdAreaTecnica)
            AND (@IdTecnico IS NULL OR IdTecnicoAsignado = @IdTecnico)
        ) AS DECIMAL(5,2)) AS Porcentaje
    FROM soporte.Caso c
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico)
    GROUP BY ec.IdEstadoCaso, ec.NombreEstadoCaso
    ORDER BY Cantidad DESC;

    -- Result Set 3: Casos por prioridad
    SELECT 
        p.IdPrioridad,
        p.NombrePrioridad,
        COUNT(*) AS Cantidad,
        COUNT(CASE WHEN ec.NombreEstadoCaso IN ('Abierto', 'Asignado', 'En Proceso', 'Escalado') THEN 1 END) AS CasosAbiertos,
        COUNT(CASE WHEN ec.NombreEstadoCaso IN ('Resuelto', 'Cerrado') THEN 1 END) AS CasosCerrados
    FROM soporte.Caso c
    INNER JOIN catalogo.Prioridad p ON c.IdPrioridad = p.IdPrioridad
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico)
    GROUP BY p.IdPrioridad, p.NombrePrioridad
    ORDER BY p.IdPrioridad;

    -- Result Set 4: Casos por técnico (top 10)
    SELECT TOP 10
        u.IdUsuario,
        u.NombreCompleto,
        COUNT(*) AS CasosAsignados,
        COUNT(CASE WHEN ec.NombreEstadoCaso = 'Resuelto' THEN 1 END) AS CasosResueltos,
        COUNT(CASE WHEN ec.NombreEstadoCaso = 'Cerrado' THEN 1 END) AS CasosCerrados,
        COUNT(CASE WHEN ec.NombreEstadoCaso IN ('Asignado', 'En Proceso') THEN 1 END) AS CasosEnProceso,
        AVG(CASE 
            WHEN c.FechaResolucion IS NOT NULL 
            THEN DATEDIFF(HOUR, c.FechaRegistro, c.FechaResolucion) 
        END) AS TiempoPromedioResolucionHoras
    FROM soporte.Caso c
    INNER JOIN acceso.Usuario u ON c.IdTecnicoAsignado = u.IdUsuario
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico)
    GROUP BY u.IdUsuario, u.NombreCompleto
    ORDER BY CasosAsignados DESC;

    -- Result Set 5: Casos por área técnica
    SELECT 
        atc.IdAreaTecnica,
        atc.NombreAreaTecnica,
        COUNT(*) AS Cantidad,
        COUNT(CASE WHEN ec.NombreEstadoCaso IN ('Abierto', 'Asignado', 'En Proceso', 'Escalado') THEN 1 END) AS CasosAbiertos,
        COUNT(CASE WHEN ec.NombreEstadoCaso IN ('Resuelto', 'Cerrado') THEN 1 END) AS CasosCerrados,
        AVG(CASE 
            WHEN c.FechaResolucion IS NOT NULL 
            THEN DATEDIFF(HOUR, c.FechaRegistro, c.FechaResolucion) 
        END) AS TiempoPromedioResolucionHoras
    FROM soporte.Caso c
    INNER JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico)
    GROUP BY atc.IdAreaTecnica, atc.NombreAreaTecnica
    ORDER BY Cantidad DESC;

    -- Result Set 6: Casos por tipo
    SELECT 
        tc.IdTipoCaso,
        tc.NombreTipoCaso,
        COUNT(*) AS Cantidad,
        CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM soporte.Caso 
            WHERE FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
            AND (@IdAreaTecnica IS NULL OR IdAreaTecnica = @IdAreaTecnica)
            AND (@IdTecnico IS NULL OR IdTecnicoAsignado = @IdTecnico)
        ) AS DECIMAL(5,2)) AS Porcentaje
    FROM soporte.Caso c
    INNER JOIN catalogo.TipoCaso tc ON c.IdTipoCaso = tc.IdTipoCaso
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico)
    GROUP BY tc.IdTipoCaso, tc.NombreTipoCaso
    ORDER BY Cantidad DESC;

    -- Result Set 7: Tendencia diaria (últimos 30 días o rango especificado)
    SELECT 
        CAST(c.FechaRegistro AS DATE) AS Fecha,
        COUNT(*) AS CasosCreados,
        COUNT(CASE WHEN c.FechaResolucion IS NOT NULL AND CAST(c.FechaResolucion AS DATE) = CAST(c.FechaRegistro AS DATE) THEN 1 END) AS CasosResueltos,
        COUNT(CASE WHEN c.FechaCierre IS NOT NULL AND CAST(c.FechaCierre AS DATE) = CAST(c.FechaRegistro AS DATE) THEN 1 END) AS CasosCerrados
    FROM soporte.Caso c
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico)
    GROUP BY CAST(c.FechaRegistro AS DATE)
    ORDER BY Fecha DESC;

    -- Result Set 8: Estado SLA
    SELECT 
        COUNT(CASE 
            WHEN ec.NombreEstadoCaso NOT IN ('Cerrado') 
            AND DATEDIFF(HOUR, c.FechaRegistro, SYSUTCDATETIME()) <= 24 
            THEN 1 
        END) AS CasosEnTiempo,
        COUNT(CASE 
            WHEN ec.NombreEstadoCaso NOT IN ('Cerrado') 
            AND DATEDIFF(HOUR, c.FechaRegistro, SYSUTCDATETIME()) BETWEEN 25 AND 48 
            THEN 1 
        END) AS CasosProximosVencer,
        COUNT(CASE 
            WHEN ec.NombreEstadoCaso NOT IN ('Cerrado') 
            AND DATEDIFF(HOUR, c.FechaRegistro, SYSUTCDATETIME()) > 48 
            THEN 1 
        END) AS CasosVencidos,
        COUNT(CASE 
            WHEN ec.NombreEstadoCaso = 'Cerrado' 
            AND DATEDIFF(HOUR, c.FechaRegistro, c.FechaCierre) <= 48 
            THEN 1 
        END) AS CasosCerradosEnSLA,
        COUNT(CASE 
            WHEN ec.NombreEstadoCaso = 'Cerrado' 
            AND DATEDIFF(HOUR, c.FechaRegistro, c.FechaCierre) > 48 
            THEN 1 
        END) AS CasosCerradosFueraSLA
    FROM soporte.Caso c
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    WHERE c.FechaRegistro BETWEEN @FechaDesde AND @FechaHasta
    AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
    AND (@IdTecnico IS NULL OR c.IdTecnicoAsignado = @IdTecnico);

END;
GO
