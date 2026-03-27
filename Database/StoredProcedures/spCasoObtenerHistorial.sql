-- =============================================
-- Stored Procedure: spCasoObtenerHistorial
-- Descripción: Obtener caso con toda su trazabilidad, intervenciones y detalles
-- Retorna múltiples result sets para consumir con Dapper
-- =============================================

CREATE OR ALTER PROCEDURE soporte.spCasoObtenerHistorial
    @IdCaso BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Validar que el caso existe
    IF NOT EXISTS (SELECT 1 FROM soporte.Caso WHERE IdCaso = @IdCaso)
        THROW 50001, 'El caso no existe.', 1;

    -- Result Set 1: Información completa del caso
    SELECT 
        c.IdCaso,
        c.NumeroCaso,
        c.Descripcion,
        c.IdUsuarioReporta,
        ur.NombreCompleto AS NombreUsuarioReporta,
        ur.Email AS EmailUsuarioReporta,
        c.TelefonoContacto,
        c.CorreoContacto,
        c.IdTecnicoAsignado,
        ta.NombreCompleto AS NombreTecnicoAsignado,
        ta.Email AS EmailTecnicoAsignado,
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
        a.Serie AS SerialActivo,
        c.FechaRegistro,
        c.FechaAceptacion,
        c.FechaResolucion,
        c.FechaCierre,
        c.FechaActualizacion,
        c.IdUsuarioCreacion,
        uc.NombreCompleto AS NombreUsuarioCreacion
    FROM soporte.Caso c
    INNER JOIN acceso.Usuario ur ON c.IdUsuarioReporta = ur.IdUsuario
    LEFT JOIN acceso.Usuario ta ON c.IdTecnicoAsignado = ta.IdUsuario
    INNER JOIN catalogo.EstadoCaso ec ON c.IdEstadoCaso = ec.IdEstadoCaso
    INNER JOIN catalogo.Prioridad p ON c.IdPrioridad = p.IdPrioridad
    INNER JOIN catalogo.TipoCaso tc ON c.IdTipoCaso = tc.IdTipoCaso
    INNER JOIN catalogo.CanalIngreso ci ON c.IdCanalIngreso = ci.IdCanalIngreso
    LEFT JOIN catalogo.AreaTecnica atc ON c.IdAreaTecnica = atc.IdAreaTecnica
    LEFT JOIN inventario.Activo a ON c.IdActivo = a.IdActivo
    LEFT JOIN acceso.Usuario uc ON c.IdUsuarioCreacion = uc.IdUsuario
    WHERE c.IdCaso = @IdCaso;

    -- Result Set 2: Trazabilidad completa (historial ordenado)
    SELECT 
        t.IdTrazabilidadCaso,
        t.FechaEvento,
        t.TipoEvento,
        t.Comentario,
        t.IdUsuarioAccion,
        u.NombreCompleto AS NombreUsuarioAccion,
        u.Email AS EmailUsuarioAccion,
        t.IdEstadoCaso,
        ec.NombreEstadoCaso,
        t.IdAreaTecnica,
        atc.NombreAreaTecnica,
        t.IdTecnicoAsignado,
        tec.NombreCompleto AS NombreTecnicoAsignado
    FROM soporte.TrazabilidadCaso t
    INNER JOIN acceso.Usuario u ON t.IdUsuarioAccion = u.IdUsuario
    LEFT JOIN catalogo.EstadoCaso ec ON t.IdEstadoCaso = ec.IdEstadoCaso
    LEFT JOIN catalogo.AreaTecnica atc ON t.IdAreaTecnica = atc.IdAreaTecnica
    LEFT JOIN acceso.Usuario tec ON t.IdTecnicoAsignado = tec.IdUsuario
    WHERE t.IdCaso = @IdCaso
    ORDER BY t.FechaEvento DESC;

    -- Result Set 3: Intervenciones técnicas
    SELECT 
        i.IdIntervencionTecnica,
        i.IdTrazabilidadCaso,
        i.IdTipoTrabajo,
        tt.NombreTipoTrabajo,
        i.IdEstadoIntervencion,
        ei.NombreEstado AS NombreEstadoIntervencion,
        i.FechaInicio,
        i.FechaFin,
        i.Diagnostico,
        i.SolucionAplicada,
        i.IdUsuarioAccion,
        u.NombreCompleto AS NombreTecnico,
        (SELECT COUNT(*) FROM soporte.DetalleCambioComponentes WHERE IdIntervencionTecnica = i.IdIntervencionTecnica) AS CantidadComponentes,
        (SELECT COUNT(*) FROM soporte.DetalleConsumible WHERE IdIntervencionTecnica = i.IdIntervencionTecnica) AS CantidadConsumibles
    FROM soporte.IntervencionTecnica i
    INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
    INNER JOIN catalogo.TipoTrabajo tt ON i.IdTipoTrabajo = tt.IdTipoTrabajo
    INNER JOIN catalogo.EstadoIntervencionTecnica ei ON i.IdEstadoIntervencion = ei.IdEstadoIntervencion
    INNER JOIN acceso.Usuario u ON i.IdUsuarioAccion = u.IdUsuario
    WHERE t.IdCaso = @IdCaso
    ORDER BY i.FechaInicio DESC;

    -- Result Set 4: Componentes usados en todas las intervenciones
    SELECT 
        dc.IdCambioComponente,
        dc.IdIntervencionTecnica,
        dc.IdComponente,
        c.NombreComponente,
        c.Marca,
        c.Modelo,
        dc.Cantidad,
        dc.TipoCambio,
        dc.DescripcionCambio,
        dc.FechaRegistro
    FROM soporte.DetalleCambioComponentes dc
    INNER JOIN inventario.Componente c ON dc.IdComponente = c.IdComponente
    INNER JOIN soporte.IntervencionTecnica i ON dc.IdIntervencionTecnica = i.IdIntervencionTecnica
    INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
    WHERE t.IdCaso = @IdCaso
    ORDER BY dc.FechaRegistro DESC;

    -- Result Set 5: Consumibles usados en todas las intervenciones
    SELECT 
        dco.IdDetalleConsumible,
        dco.IdIntervencionTecnica,
        dco.IdConsumible,
        co.NombreConsumible,
        co.Marca,
        dco.Cantidad,
        dco.DescripcionUso,
        dco.FechaRegistro
    FROM soporte.DetalleConsumible dco
    INNER JOIN inventario.Consumible co ON dco.IdConsumible = co.IdConsumible
    INNER JOIN soporte.IntervencionTecnica i ON dco.IdIntervencionTecnica = i.IdIntervencionTecnica
    INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
    WHERE t.IdCaso = @IdCaso
    ORDER BY dco.FechaRegistro DESC;

    -- Result Set 6: Revisiones administrativas (si existen)
    SELECT 
        ra.IdRevisionAdmi,
        ra.IdIntervencionTecnica,
        ra.Aprobado,
        ra.TipoRevision,
        ra.ObservacionRevision,
        ra.FechaRegistro,
        ra.IdUsuarioCreacion,
        u.NombreCompleto AS NombreRevisor
    FROM soporte.RevisionAdmi ra
    INNER JOIN soporte.IntervencionTecnica i ON ra.IdIntervencionTecnica = i.IdIntervencionTecnica
    INNER JOIN soporte.TrazabilidadCaso t ON i.IdTrazabilidadCaso = t.IdTrazabilidadCaso
    INNER JOIN acceso.Usuario u ON ra.IdUsuarioCreacion = u.IdUsuario
    WHERE t.IdCaso = @IdCaso
    ORDER BY ra.FechaRegistro DESC;

    -- Result Set 7: Encuestas de calidad (si existen)
    SELECT 
        e.IdEncuesta,
        e.IdCaso,
        e.FechaEncuesta,
        e.Observaciones,
        e.IdUsuarioCreacion,
        u.NombreCompleto AS NombreUsuario,
        (SELECT COUNT(*) FROM soporte.DetalleEncuesta WHERE IdEncuesta = e.IdEncuesta) AS CantidadRespuestas
    FROM soporte.EncuestaCalidad e
    INNER JOIN acceso.Usuario u ON e.IdUsuarioCreacion = u.IdUsuario
    WHERE e.IdCaso = @IdCaso
    ORDER BY e.FechaEncuesta DESC;

END;
GO
