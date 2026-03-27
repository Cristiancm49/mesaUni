-- =============================================
-- Stored Procedure: spCasoObtenerPorFiltros
-- Descripción: Obtener casos con filtros múltiples y paginación
-- =============================================

CREATE OR ALTER PROCEDURE soporte.spCasoObtenerPorFiltros
    @IdEstadoCaso BIGINT = NULL,
    @IdTecnicoAsignado BIGINT = NULL,
    @IdAreaTecnica BIGINT = NULL,
    @IdPrioridad BIGINT = NULL,
    @IdUsuarioReporta BIGINT = NULL,
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL,
    @TextoBusqueda VARCHAR(500) = NULL,
    @Page INT = 1,
    @PageSize INT = 20,
    @OrderBy VARCHAR(50) = 'FechaRegistro',  -- FechaRegistro, Prioridad, Estado
    @OrderDirection VARCHAR(4) = 'DESC'  -- ASC o DESC
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    -- Result Set 1: Total de registros (para paginación)
    SELECT COUNT(*) AS TotalRegistros
    FROM soporte.Caso c
    WHERE 
        (@IdEstadoCaso IS NULL OR c.IdEstadoCaso = @IdEstadoCaso)
        AND (@IdTecnicoAsignado IS NULL OR c.IdTecnicoAsignado = @IdTecnicoAsignado)
        AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
        AND (@IdPrioridad IS NULL OR c.IdPrioridad = @IdPrioridad)
        AND (@IdUsuarioReporta IS NULL OR c.IdUsuarioReporta = @IdUsuarioReporta)
        AND (@FechaDesde IS NULL OR c.FechaRegistro >= @FechaDesde)
        AND (@FechaHasta IS NULL OR c.FechaRegistro <= @FechaHasta)
        AND (
            @TextoBusqueda IS NULL 
            OR c.Descripcion LIKE '%' + @TextoBusqueda + '%'
            OR c.NumeroCaso LIKE '%' + @TextoBusqueda + '%'
        );

    -- Result Set 2: Casos paginados
    SELECT 
        c.IdCaso,
        c.NumeroCaso,
        c.Descripcion,
        c.IdUsuarioReporta,
        ur.NombreCompleto AS NombreUsuarioReporta,
        c.TelefonoContacto,
        c.CorreoContacto,
        c.IdTecnicoAsignado,
        ta.NombreCompleto AS NombreTecnicoAsignado,
        c.IdEstadoCaso,
        ec.NombreEstadoCaso,
        c.IdPrioridad,
        p.NombrePrioridad,
        c.IdTipoCaso,
        tc.NombreTipoCaso,
        c.IdCanalIngreso,
        ci.NombreCanal,
        c.IdAreaTecnica,
        atc.NombreAreaTecnica,
        c.IdActivo,
        a.NombreActivo,
        c.FechaRegistro,
        c.FechaAceptacion,
        c.FechaResolucion,
        c.FechaCierre,
        c.FechaActualizacion,
        DATEDIFF(HOUR, c.FechaRegistro, COALESCE(c.FechaCierre, SYSUTCDATETIME())) AS HorasTranscurridas,
        CASE 
            WHEN c.FechaCierre IS NOT NULL THEN 'Cerrado'
            WHEN DATEDIFF(HOUR, c.FechaRegistro, SYSUTCDATETIME()) > 48 THEN 'Vencido'
            WHEN DATEDIFF(HOUR, c.FechaRegistro, SYSUTCDATETIME()) > 24 THEN 'Proximo a vencer'
            ELSE 'En tiempo'
        END AS EstadoSLA
    FROM soporte.Caso c
    INNER JOIN acceso.Usuario ur ON c.IdUsuarioReporta = ur.IdUsuario
    LEFT JOIN acceso.Usuario ta ON c.IdTecnicoAsignado = ta.IdUsuario
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    INNER JOIN catalogo.Prioridad p ON c.IdPrioridad = p.IdPrioridad
    INNER JOIN catalogo.TipoCaso tc ON c.IdTipoCaso = tc.IdTipoCaso
    INNER JOIN catalogo.CanalIngreso ci ON c.IdCanalIngreso = ci.IdCanalIngreso
    LEFT JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
    LEFT JOIN inventario.Activo a ON c.IdActivo = a.IdActivo
    WHERE 
        (@IdEstadoCaso IS NULL OR c.IdEstadoCaso = @IdEstadoCaso)
        AND (@IdTecnicoAsignado IS NULL OR c.IdTecnicoAsignado = @IdTecnicoAsignado)
        AND (@IdAreaTecnica IS NULL OR c.IdAreaTecnica = @IdAreaTecnica)
        AND (@IdPrioridad IS NULL OR c.IdPrioridad = @IdPrioridad)
        AND (@IdUsuarioReporta IS NULL OR c.IdUsuarioReporta = @IdUsuarioReporta)
        AND (@FechaDesde IS NULL OR c.FechaRegistro >= @FechaDesde)
        AND (@FechaHasta IS NULL OR c.FechaRegistro <= @FechaHasta)
        AND (
            @TextoBusqueda IS NULL 
            OR c.Descripcion LIKE '%' + @TextoBusqueda + '%'
            OR c.NumeroCaso LIKE '%' + @TextoBusqueda + '%'
        )
    ORDER BY 
        CASE WHEN @OrderBy = 'FechaRegistro' AND @OrderDirection = 'DESC' THEN c.FechaRegistro END DESC,
        CASE WHEN @OrderBy = 'FechaRegistro' AND @OrderDirection = 'ASC' THEN c.FechaRegistro END ASC,
        CASE WHEN @OrderBy = 'Prioridad' AND @OrderDirection = 'DESC' THEN c.IdPrioridad END DESC,
        CASE WHEN @OrderBy = 'Prioridad' AND @OrderDirection = 'ASC' THEN c.IdPrioridad END ASC,
        CASE WHEN @OrderBy = 'Estado' AND @OrderDirection = 'DESC' THEN c.IdEstadoCaso END DESC,
        CASE WHEN @OrderBy = 'Estado' AND @OrderDirection = 'ASC' THEN c.IdEstadoCaso END ASC,
        c.FechaRegistro DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

END;
GO
