using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Reportes;
using MicroApi.Seguridad.Domain.Interfaces;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class ReporteConfiguracionRepository : IReporteConfiguracionRepository
    {
        private readonly ApplicationDbContext _context;

        private const string QuerySql = @"
WITH AuditBase AS (
    SELECT
        audit.AuditId,
        audit.Fecha,
        audit.Accion,
        audit.Entidad,
        audit.EntidadId,
        audit.EntidadNombre,
        audit.ChangedBy,
        audit.SessionUserId,
        audit.HostName,
        audit.AppName,
        audit.CorrelationId,
        audit.PrimaryKeyJson,
        audit.OldValuesJson,
        audit.NewValuesJson,
        COALESCE(actorById.Email, actorByEmail.Email, NULLIF(audit.ChangedBy, N''), N'Sistema') AS Usuario,
        COALESCE(actorById.NombreCompleto, actorByEmail.NombreCompleto, NULLIF(audit.ChangedBy, N''), N'Sistema') AS NombreUsuario,
        COALESCE(roleById.NombreRol, roleByEmail.NombreRol, N'Sistema') AS NombreRol,
        CASE
            WHEN LOWER(COALESCE(roleById.NombreRol, roleByEmail.NombreRol, N'')) LIKE N'%admin%' THEN N'admin'
            WHEN LOWER(COALESCE(roleById.NombreRol, roleByEmail.NombreRol, N'')) LIKE N'%tecn%' THEN N'tecnico'
            WHEN LOWER(COALESCE(roleById.NombreRol, roleByEmail.NombreRol, N'')) LIKE N'%usuar%' THEN N'usuario'
            WHEN NULLIF(audit.ChangedBy, N'') IS NULL THEN N'sistema'
            ELSE N'otro'
        END AS Rol,
        CAST(N'success' AS NVARCHAR(20)) AS Resultado
    FROM auditoria.vwReporteConfiguracionBase audit
    LEFT JOIN acceso.Usuario actorById
        ON actorById.IdUsuario = audit.SessionUserId
    LEFT JOIN acceso.Rol roleById
        ON roleById.IdRol = actorById.IdRol
    LEFT JOIN acceso.Usuario actorByEmail
        ON actorByEmail.Email = audit.ChangedBy
    LEFT JOIN acceso.Rol roleByEmail
        ON roleByEmail.IdRol = actorByEmail.IdRol
)
SELECT *
INTO #AuditBase
FROM AuditBase;

DECLARE @UsuarioFiltro NVARCHAR(256) = LOWER(LTRIM(RTRIM(ISNULL(@Usuario, N''))));
DECLARE @AccionFiltro NVARCHAR(256) = UPPER(LTRIM(RTRIM(ISNULL(@Accion, N''))));
DECLARE @EntidadFiltro NVARCHAR(256) = UPPER(LTRIM(RTRIM(ISNULL(@Entidad, N''))));
DECLARE @ResultadoFiltro NVARCHAR(50) = LOWER(LTRIM(RTRIM(ISNULL(@Resultado, N''))));
DECLARE @RolFiltro NVARCHAR(50) = LOWER(LTRIM(RTRIM(ISNULL(@Rol, N''))));
DECLARE @EquipoFiltro NVARCHAR(256) = LOWER(LTRIM(RTRIM(ISNULL(@Equipo, N''))));
DECLARE @BusquedaFiltro NVARCHAR(256) = LTRIM(RTRIM(ISNULL(@Busqueda, N'')));

SELECT COUNT(1) AS TotalHistorico
FROM #AuditBase;

SELECT *
INTO #AuditFiltered
FROM #AuditBase
WHERE (
        @UsuarioFiltro = N''
        OR LOWER(LTRIM(RTRIM(ISNULL(Usuario, N'')))) LIKE N'%' + @UsuarioFiltro + N'%'
        OR LOWER(LTRIM(RTRIM(ISNULL(NombreUsuario, N'')))) LIKE N'%' + @UsuarioFiltro + N'%'
    )
  AND (@AccionFiltro = N'' OR UPPER(LTRIM(RTRIM(ISNULL(Accion, N'')))) = @AccionFiltro)
  AND (@EntidadFiltro = N'' OR UPPER(LTRIM(RTRIM(ISNULL(Entidad, N'')))) = @EntidadFiltro)
  AND (@ResultadoFiltro = N'' OR LOWER(LTRIM(RTRIM(ISNULL(Resultado, N'')))) = @ResultadoFiltro)
  AND (@RolFiltro = N'' OR LOWER(LTRIM(RTRIM(ISNULL(Rol, N'')))) = @RolFiltro)
  AND (
        @EquipoFiltro = N''
        OR LOWER(CONCAT(ISNULL(HostName, N''), N' ', ISNULL(AppName, N''))) LIKE N'%' + @EquipoFiltro + N'%'
        OR LOWER(
            CASE
                WHEN (
                    LOWER(ISNULL(HostName, N'')) = N'backend'
                    OR LOWER(ISNULL(AppName, N'')) = N'backend'
                    OR LOWER(CONCAT(ISNULL(HostName, N''), N' ', ISNULL(AppName, N''))) LIKE N'%registro desde backend%'
                    OR LOWER(CONCAT(ISNULL(HostName, N''), N' ', ISNULL(AppName, N''))) LIKE N'%carga desde catalogo backend%'
                ) THEN N'api mesa de servicios'
                WHEN LOWER(CONCAT(ISNULL(HostName, N''), N' ', ISNULL(AppName, N''))) LIKE N'%swagger%' THEN N'swagger'
                WHEN LOWER(CONCAT(ISNULL(HostName, N''), N' ', ISNULL(AppName, N''))) LIKE N'%postman%' THEN N'postman'
                WHEN LOWER(CONCAT(ISNULL(HostName, N''), N' ', ISNULL(AppName, N''))) LIKE N'%mozilla%' THEN N'navegador web'
                WHEN LOWER(CONCAT(ISNULL(HostName, N''), N' ', ISNULL(AppName, N''))) LIKE N'%mongo%' THEN N'script de mongodb'
                WHEN NULLIF(LTRIM(RTRIM(CONCAT(ISNULL(HostName, N''), ISNULL(AppName, N'')))), N'') IS NULL THEN N'no disponible'
                ELSE LOWER(CONCAT(ISNULL(HostName, N''), N' | ', ISNULL(AppName, N'')))
            END
        ) LIKE N'%' + @EquipoFiltro + N'%'
    )
  AND (@FechaDesde IS NULL OR Fecha >= @FechaDesde)
  AND (@FechaHasta IS NULL OR Fecha < DATEADD(DAY, 1, @FechaHasta))
  AND (
        @BusquedaFiltro = N''
        OR CONCAT(
            ISNULL(EntidadNombre, N''), N' ',
            ISNULL(Entidad, N''), N' ',
            ISNULL(Accion, N''), N' ',
            ISNULL(NombreUsuario, N''), N' ',
            ISNULL(Usuario, N''), N' ',
            ISNULL(HostName, N''), N' ',
            ISNULL(AppName, N''), N' ',
            ISNULL(CorrelationId, N'')
        ) LIKE N'%' + @BusquedaFiltro + N'%'
    );

SELECT
    AuditId,
    Fecha,
    Usuario,
    NombreUsuario,
    Accion,
    Entidad,
    EntidadId,
    EntidadNombre,
    Rol,
    Resultado,
    HostName,
    AppName,
    CorrelationId,
    PrimaryKeyJson,
    OldValuesJson,
    NewValuesJson
FROM #AuditFiltered
ORDER BY Fecha DESC, AuditId DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT(1) AS TotalItems
FROM #AuditFiltered;

SELECT
    COUNT(1) AS TotalCambios,
    SUM(CASE WHEN Fecha >= DATEADD(DAY, DATEDIFF(DAY, 0, SYSUTCDATETIME()), 0) THEN 1 ELSE 0 END) AS CambiosHoy,
    SUM(CASE WHEN Fecha >= DATEADD(DAY, -7, SYSUTCDATETIME()) THEN 1 ELSE 0 END) AS CambiosEstaSemana,
    SUM(CASE WHEN Fecha >= DATEADD(MONTH, -1, SYSUTCDATETIME()) THEN 1 ELSE 0 END) AS CambiosEsteMes,
    COUNT(DISTINCT NULLIF(Usuario, N'')) AS UsuariosActivos
FROM #AuditFiltered;

SELECT TOP (10)
    Accion AS Clave,
    COUNT(1) AS Cantidad
FROM #AuditFiltered
GROUP BY Accion
ORDER BY COUNT(1) DESC, Accion ASC;

SELECT TOP (10)
    Entidad AS Clave,
    COUNT(1) AS Cantidad
FROM #AuditFiltered
GROUP BY Entidad
ORDER BY COUNT(1) DESC, Entidad ASC;

SELECT TOP (10)
    Usuario,
    NombreUsuario,
    Rol,
    COUNT(1) AS Cantidad
FROM #AuditFiltered
GROUP BY Usuario, NombreUsuario, Rol
ORDER BY COUNT(1) DESC, NombreUsuario ASC;

SELECT
    trend.Fecha,
    trend.Cantidad
FROM (
    SELECT TOP (10)
        CONVERT(date, Fecha) AS Fecha,
        COUNT(1) AS Cantidad
    FROM #AuditFiltered
    GROUP BY CONVERT(date, Fecha)
    ORDER BY CONVERT(date, Fecha) DESC
) trend
ORDER BY trend.Fecha ASC;

SELECT
    Usuario,
    MAX(NombreUsuario) AS NombreUsuario,
    MAX(Rol) AS Rol
FROM #AuditBase
WHERE NULLIF(Usuario, N'') IS NOT NULL
GROUP BY Usuario
ORDER BY MAX(NombreUsuario) ASC, Usuario ASC;

SELECT DISTINCT Accion
FROM #AuditBase
ORDER BY Accion ASC;

SELECT DISTINCT Entidad
FROM #AuditBase
ORDER BY Entidad ASC;

DROP TABLE #AuditFiltered;
DROP TABLE #AuditBase;";

        public ReporteConfiguracionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConfiguracionAuditReportDto> GetAuditoriaAsync(ConfiguracionAuditQueryDto query)
        {
            var page = Math.Max(query.Page, 1);
            var pageSize = Math.Clamp(query.PageSize, 5, 100);
            var offset = (page - 1) * pageSize;
            var report = new ConfiguracionAuditReportDto
            {
                Registros = new PagedResponseDto<ConfiguracionAuditLogDto>
                {
                    Page = page,
                    PageSize = pageSize
                }
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = QuerySql;
            command.CommandType = CommandType.Text;
            command.Parameters.Add(CreateParameter(command, "@Usuario", query.Usuario));
            command.Parameters.Add(CreateParameter(command, "@Accion", query.Accion));
            command.Parameters.Add(CreateParameter(command, "@Entidad", query.Entidad));
            command.Parameters.Add(CreateParameter(command, "@Resultado", query.Resultado));
            command.Parameters.Add(CreateParameter(command, "@Rol", query.Rol));
            command.Parameters.Add(CreateParameter(command, "@Equipo", query.Equipo));
            command.Parameters.Add(CreateParameter(command, "@Busqueda", query.Busqueda));
            command.Parameters.Add(CreateParameter(command, "@FechaDesde", query.FechaDesde));
            command.Parameters.Add(CreateParameter(command, "@FechaHasta", query.FechaHasta));
            command.Parameters.Add(CreateParameter(command, "@Offset", offset));
            command.Parameters.Add(CreateParameter(command, "@PageSize", pageSize));

            var shouldClose = _context.Database.GetDbConnection().State != ConnectionState.Open;
            if (shouldClose)
            {
                await _context.Database.OpenConnectionAsync();
            }

            try
            {
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    report.TotalHistorico = GetInt32(reader, "TotalHistorico");
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.Registros.Items.Add(MapLog(reader));
                    }
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    report.Registros.TotalItems = GetInt32(reader, "TotalItems");
                }

                if (await reader.NextResultAsync() && await reader.ReadAsync())
                {
                    report.Estadisticas = new ConfiguracionAuditSummaryDto
                    {
                        TotalCambios = GetInt32(reader, "TotalCambios"),
                        CambiosHoy = GetInt32(reader, "CambiosHoy"),
                        CambiosEstaSemana = GetInt32(reader, "CambiosEstaSemana"),
                        CambiosEsteMes = GetInt32(reader, "CambiosEsteMes"),
                        UsuariosActivos = GetInt32(reader, "UsuariosActivos"),
                    };
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.AccionesMasComunes.Add(new ConfiguracionAuditCountDto
                        {
                            Clave = GetString(reader, "Clave"),
                            Cantidad = GetInt32(reader, "Cantidad"),
                        });
                    }
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.EntidadesMasModificadas.Add(new ConfiguracionAuditCountDto
                        {
                            Clave = GetString(reader, "Clave"),
                            Cantidad = GetInt32(reader, "Cantidad"),
                        });
                    }
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.UsuariosMasActivos.Add(new ConfiguracionAuditUserActivityDto
                        {
                            Usuario = GetString(reader, "Usuario"),
                            NombreUsuario = GetString(reader, "NombreUsuario", GetString(reader, "Usuario")),
                            Rol = GetString(reader, "Rol", "sistema"),
                            Cantidad = GetInt32(reader, "Cantidad"),
                        });
                    }
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var fecha = GetDateTime(reader, "Fecha");
                        report.TendenciaCambios.Add(new ConfiguracionAuditTrendDto
                        {
                            Fecha = fecha,
                            Etiqueta = fecha.ToString("dd MMM", new CultureInfo("es-CO")).Replace(".", string.Empty),
                            Cantidad = GetInt32(reader, "Cantidad"),
                        });
                    }
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.Usuarios.Add(new ConfiguracionAuditUserOptionDto
                        {
                            Usuario = GetString(reader, "Usuario"),
                            NombreUsuario = GetString(reader, "NombreUsuario", GetString(reader, "Usuario")),
                            Rol = GetString(reader, "Rol", "sistema"),
                        });
                    }
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.Acciones.Add(GetString(reader, "Accion"));
                    }
                }

                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        report.Entidades.Add(GetString(reader, "Entidad"));
                    }
                }
            }
            finally
            {
                if (shouldClose)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return report;
        }

        private static ConfiguracionAuditLogDto MapLog(IDataRecord reader)
        {
            var auditId = GetInt64(reader, "AuditId");
            var accion = GetString(reader, "Accion");
            var entidad = GetString(reader, "Entidad");
            var entidadNombre = GetString(reader, "EntidadNombre", $"{Prettify(entidad)} {auditId}");
            var hostName = GetString(reader, "HostName");
            var appName = GetString(reader, "AppName");

            return new ConfiguracionAuditLogDto
            {
                Id = $"AUD-{auditId}",
                Fecha = GetDateTime(reader, "Fecha"),
                Usuario = GetString(reader, "Usuario", "Sistema"),
                NombreUsuario = GetString(reader, "NombreUsuario", GetString(reader, "Usuario", "Sistema")),
                Accion = accion,
                Entidad = entidad,
                EntidadId = GetNullableInt64(reader, "EntidadId") ?? 0,
                EntidadNombre = entidadNombre,
                Cambios = new ConfiguracionAuditChangesDto
                {
                    Anterior = ParseJsonPayload(GetString(reader, "OldValuesJson")),
                    Nuevo = ParseJsonPayload(GetString(reader, "NewValuesJson")),
                },
                Ip = hostName,
                UserAgent = BuildDeviceOrigin(hostName, appName),
                Detalles = BuildDetail(accion, entidad, entidadNombre),
                Resultado = GetString(reader, "Resultado", "success"),
                Rol = GetString(reader, "Rol", "sistema"),
                HostName = hostName,
                AppName = appName,
                CorrelationId = GetString(reader, "CorrelationId"),
                PrimaryKeyJson = GetString(reader, "PrimaryKeyJson"),
            };
        }

        private static DbParameter CreateParameter(DbCommand command, string name, object? value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        private static int GetInt32(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        private static long GetInt64(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? 0 : Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        private static long? GetNullableInt64(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? null : Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        private static DateTime GetDateTime(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(value, CultureInfo.InvariantCulture);
        }

        private static string GetString(IDataRecord reader, string column, string fallback = "")
        {
            var value = reader[column];
            if (value == DBNull.Value)
            {
                return fallback;
            }

            var text = Convert.ToString(value)?.Trim();
            return string.IsNullOrWhiteSpace(text) ? fallback : text;
        }

        private static object? ParseJsonPayload(string? rawJson)
        {
            if (string.IsNullOrWhiteSpace(rawJson))
            {
                return null;
            }

            try
            {
                using var document = JsonDocument.Parse(rawJson);
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    if (root.GetArrayLength() == 0)
                    {
                        return null;
                    }

                    if (root.GetArrayLength() == 1)
                    {
                        return JsonSerializer.Deserialize<object>(root[0].GetRawText());
                    }
                }

                return JsonSerializer.Deserialize<object>(root.GetRawText());
            }
            catch
            {
                return rawJson;
            }
        }

        private static string BuildDetail(string accion, string entidad, string entidadNombre)
        {
            var verbo = accion.StartsWith("CREAR", StringComparison.OrdinalIgnoreCase)
                ? "Se creo"
                : accion.StartsWith("MODIFICAR", StringComparison.OrdinalIgnoreCase)
                    ? "Se actualizo"
                    : accion.StartsWith("ELIMINAR", StringComparison.OrdinalIgnoreCase)
                        ? "Se elimino"
                        : "Se registro un cambio sobre";

            return $"{verbo} {entidadNombre} en {Prettify(entidad)}.";
        }

        private static string BuildDeviceOrigin(string hostName, string appName)
        {
            var host = hostName.Trim();
            var app = appName.Trim();

            if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(app))
            {
                return $"{host} | {app}";
            }

            if (!string.IsNullOrWhiteSpace(host))
            {
                return host;
            }

            if (!string.IsNullOrWhiteSpace(app))
            {
                return app;
            }

            return "N/D";
        }

        private static string Prettify(string value)
        {
            var text = (value ?? string.Empty)
                .Replace('_', ' ')
                .Replace('-', ' ')
                .Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                return "configuracion";
            }

            var builder = new StringBuilder(text.Length);
            var upperNext = true;

            foreach (var character in text.ToLowerInvariant())
            {
                if (upperNext && char.IsLetter(character))
                {
                    builder.Append(char.ToUpperInvariant(character));
                    upperNext = false;
                }
                else
                {
                    builder.Append(character);
                    upperNext = char.IsWhiteSpace(character);
                }
            }

            return builder.ToString();
        }
    }
}
