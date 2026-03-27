/*
    Fuente real para el reporte de configuracion.
    Consolida la auditoria del schema `auditoria` sobre entidades
    de acceso y catalogos que forman parte de la configuracion del sistema.
*/

SET NOCOUNT ON;
GO

CREATE OR ALTER VIEW auditoria.vwReporteConfiguracionBase
AS
    SELECT
        audit.AuditId,
        audit.EventDateUtc AS Fecha,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_USUARIO'
            WHEN 'U' THEN 'MODIFICAR_USUARIO'
            ELSE 'ELIMINAR_USUARIO'
        END AS Accion,
        CAST('USUARIO' AS VARCHAR(80)) AS Entidad,
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdUsuario')) AS EntidadId,
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].Email'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].Email'))), ''),
            CONCAT('Usuario ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdUsuario'), 'N/D'))
        ) AS EntidadNombre,
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.acceso_Usuario_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_ROL'
            WHEN 'U' THEN 'MODIFICAR_ROL'
            ELSE 'ELIMINAR_ROL'
        END,
        CAST('ROL' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdRol')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreRol'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreRol'))), ''),
            CONCAT('Rol ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdRol'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.acceso_Rol_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_ESTADO_GENERAL'
            WHEN 'U' THEN 'MODIFICAR_ESTADO_GENERAL'
            ELSE 'ELIMINAR_ESTADO_GENERAL'
        END,
        CAST('ESTADO_GENERAL' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoGeneral')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreEstado'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreEstado'))), ''),
            CONCAT('Estado general ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoGeneral'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_EstadoGeneral_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_AREA_TECNICA'
            WHEN 'U' THEN 'MODIFICAR_AREA_TECNICA'
            ELSE 'ELIMINAR_AREA_TECNICA'
        END,
        CAST('AREA_TECNICA' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdAreaTecnica')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreAreaTecnica'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreAreaTecnica'))), ''),
            CONCAT('Area tecnica ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdAreaTecnica'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_AreaTecnica_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_TIPO_TRABAJO'
            WHEN 'U' THEN 'MODIFICAR_TIPO_TRABAJO'
            ELSE 'ELIMINAR_TIPO_TRABAJO'
        END,
        CAST('TIPO_TRABAJO' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdTipoTrabajo')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreTipoTrabajo'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreTipoTrabajo'))), ''),
            CONCAT('Tipo de trabajo ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdTipoTrabajo'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_TipoTrabajo_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_ESTADO_INTERVENCION'
            WHEN 'U' THEN 'MODIFICAR_ESTADO_INTERVENCION'
            ELSE 'ELIMINAR_ESTADO_INTERVENCION'
        END,
        CAST('ESTADO_INTERVENCION' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoIntervencion')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreEstado'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreEstado'))), ''),
            CONCAT('Estado de intervencion ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoIntervencion'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_EstadoIntervencionTecnica_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_ESTADO_CASO'
            WHEN 'U' THEN 'MODIFICAR_ESTADO_CASO'
            ELSE 'ELIMINAR_ESTADO_CASO'
        END,
        CAST('ESTADO_CASO' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoCaso')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreEstadoCaso'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreEstadoCaso'))), ''),
            CONCAT('Estado de caso ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoCaso'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_EstadoCaso_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_PRIORIDAD'
            WHEN 'U' THEN 'MODIFICAR_PRIORIDAD'
            ELSE 'ELIMINAR_PRIORIDAD'
        END,
        CAST('PRIORIDAD' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdPrioridad')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombrePrioridad'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombrePrioridad'))), ''),
            CONCAT('Prioridad ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdPrioridad'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_Prioridad_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_TIPO_CASO'
            WHEN 'U' THEN 'MODIFICAR_TIPO_CASO'
            ELSE 'ELIMINAR_TIPO_CASO'
        END,
        CAST('TIPO_CASO' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdTipoCaso')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreTipoCaso'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreTipoCaso'))), ''),
            CONCAT('Tipo de caso ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdTipoCaso'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_TipoCaso_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_CANAL_INGRESO'
            WHEN 'U' THEN 'MODIFICAR_CANAL_INGRESO'
            ELSE 'ELIMINAR_CANAL_INGRESO'
        END,
        CAST('CANAL_INGRESO' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdCanalIngreso')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreCanal'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreCanal'))), ''),
            CONCAT('Canal de ingreso ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdCanalIngreso'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_CanalIngreso_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_SEDE'
            WHEN 'U' THEN 'MODIFICAR_SEDE'
            ELSE 'ELIMINAR_SEDE'
        END,
        CAST('SEDE' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdSede')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreSede'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreSede'))), ''),
            CONCAT('Sede ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdSede'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_Sede_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_CATEGORIA_ACTIVO'
            WHEN 'U' THEN 'MODIFICAR_CATEGORIA_ACTIVO'
            ELSE 'ELIMINAR_CATEGORIA_ACTIVO'
        END,
        CAST('CATEGORIA_ACTIVO' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdCategoriaActivo')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreCategoria'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreCategoria'))), ''),
            CONCAT('Categoria de activo ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdCategoriaActivo'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_CategoriaActivo_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_ESTADO_ACTIVO'
            WHEN 'U' THEN 'MODIFICAR_ESTADO_ACTIVO'
            ELSE 'ELIMINAR_ESTADO_ACTIVO'
        END,
        CAST('ESTADO_ACTIVO' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoActivo')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreEstado'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreEstado'))), ''),
            CONCAT('Estado de activo ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoActivo'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_EstadoActivo_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_TIPO_CONSUMIBLE'
            WHEN 'U' THEN 'MODIFICAR_TIPO_CONSUMIBLE'
            ELSE 'ELIMINAR_TIPO_CONSUMIBLE'
        END,
        CAST('TIPO_CONSUMIBLE' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdTipoConsumible')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreTipo'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreTipo'))), ''),
            CONCAT('Tipo de consumible ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdTipoConsumible'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_TipoConsumible_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_ESTADO_CONSUMIBLE'
            WHEN 'U' THEN 'MODIFICAR_ESTADO_CONSUMIBLE'
            ELSE 'ELIMINAR_ESTADO_CONSUMIBLE'
        END,
        CAST('ESTADO_CONSUMIBLE' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoConsumible')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].NombreEstado'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].NombreEstado'))), ''),
            CONCAT('Estado de consumible ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdEstadoConsumible'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_EstadoConsumible_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_PREGUNTA'
            WHEN 'U' THEN 'MODIFICAR_PREGUNTA'
            ELSE 'ELIMINAR_PREGUNTA'
        END,
        CAST('PREGUNTA' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdPregunta')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].TextoPregunta'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].TextoPregunta'))), ''),
            CONCAT('Pregunta ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdPregunta'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_Pregunta_Audit audit

    UNION ALL

    SELECT
        audit.AuditId,
        audit.EventDateUtc,
        CASE audit.ActionType
            WHEN 'I' THEN 'CREAR_RESPUESTA'
            WHEN 'U' THEN 'MODIFICAR_RESPUESTA'
            ELSE 'ELIMINAR_RESPUESTA'
        END,
        CAST('RESPUESTA' AS VARCHAR(80)),
        TRY_CONVERT(BIGINT, JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdRespuesta')),
        COALESCE(
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.NewValuesJson, '$[0].TextoRespuesta'))), ''),
            NULLIF(LTRIM(RTRIM(JSON_VALUE(audit.OldValuesJson, '$[0].TextoRespuesta'))), ''),
            CONCAT('Respuesta ', COALESCE(JSON_VALUE(audit.PrimaryKeyJson, '$[0].IdRespuesta'), 'N/D'))
        ),
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson
    FROM auditoria.catalogo_Respuesta_Audit audit;
GO
