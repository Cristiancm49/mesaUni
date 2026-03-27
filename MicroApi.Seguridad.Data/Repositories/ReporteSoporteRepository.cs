using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.DTOs.Reportes;
using MicroApi.Seguridad.Domain.Interfaces;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class ReporteSoporteRepository : IReporteSoporteRepository
    {
        private readonly ApplicationDbContext _context;

        public ReporteSoporteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReporteCasoDto>> GetCasosAsync()
        {
            var casos = new List<ReporteCasoDto>();

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = @"
SELECT
    IdCaso,
    IdUsuarioReporta,
    IdTecnicoAsignado,
    IdEstadoCaso,
    IdTipoCaso,
    IdAreaTecnica,
    IdPrioridad,
    IdActivo,
    NumeroCaso,
    Descripcion,
    FechaRegistro,
    FechaResolucion,
    FechaCierre,
    FechaFinCaso,
    NombreEstadoCaso,
    NombreTipoCaso,
    NombrePrioridad,
    NombreUsuarioReporta,
    CorreoContacto,
    NombreTecnicoAsignado,
    CorreoTecnicoAsignado,
    NombreAreaTecnica,
    HorasTranscurridas,
    HorasObjetivoResolucion,
    PromedioEncuesta,
    NombreActivo,
    UbicacionTexto,
    CodigoPatrimonial,
    Marca,
    Modelo,
    FueEscalado,
    MotivoEscalado
FROM soporte.vwReporteCasosBase
ORDER BY FechaRegistro DESC, IdCaso DESC;";
            command.CommandType = CommandType.Text;

            var shouldClose = _context.Database.GetDbConnection().State != ConnectionState.Open;
            if (shouldClose)
            {
                await _context.Database.OpenConnectionAsync();
            }

            try
            {
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var descripcion = GetString(reader, "Descripcion");
                    var fechaRegistro = GetDateTime(reader, "FechaRegistro");
                    var fechaFin = GetNullableDateTime(reader, "FechaFinCaso");
                    var horasAbiertas = GetDouble(reader, "HorasTranscurridas");
                    var horasObjetivo = GetNullableDouble(reader, "HorasObjetivoResolucion");
                    var retrasado = horasObjetivo.HasValue && horasObjetivo.Value > 0 && horasAbiertas > horasObjetivo.Value;

                    casos.Add(new ReporteCasoDto
                    {
                        Id = GetInt64(reader, "IdCaso"),
                        IdUsuarioReporta = GetInt64(reader, "IdUsuarioReporta"),
                        IdTecnicoAsignado = GetNullableInt64(reader, "IdTecnicoAsignado"),
                        IdEstadoCaso = GetInt64(reader, "IdEstadoCaso"),
                        IdTipoCaso = GetInt64(reader, "IdTipoCaso"),
                        IdAreaTecnica = GetNullableInt64(reader, "IdAreaTecnica"),
                        IdPrioridad = GetInt64(reader, "IdPrioridad"),
                        IdActivo = GetNullableInt64(reader, "IdActivo"),
                        NumeroCaso = GetString(reader, "NumeroCaso"),
                        Titulo = BuildTitle(descripcion, GetInt64(reader, "IdCaso")),
                        Descripcion = descripcion,
                        FechaRegistro = fechaRegistro,
                        FechaResolucion = GetNullableDateTime(reader, "FechaResolucion"),
                        FechaCierre = GetNullableDateTime(reader, "FechaCierre"),
                        FechaFin = fechaFin,
                        EstadoEspecifico = GetString(reader, "NombreEstadoCaso"),
                        Estado = MapEstado(GetString(reader, "NombreEstadoCaso")),
                        TipoCaso = GetString(reader, "NombreTipoCaso", "General"),
                        Prioridad = MapPrioridad(GetString(reader, "NombrePrioridad")),
                        NombreUsuario = GetString(reader, "NombreUsuarioReporta", $"Usuario {GetInt64(reader, "IdUsuarioReporta")}"),
                        UsuarioReporta = GetString(reader, "CorreoContacto"),
                        NombreTecnico = GetString(reader, "NombreTecnicoAsignado", "Sin asignar"),
                        TecnicoAsignado = GetString(reader, "CorreoTecnicoAsignado", GetString(reader, "NombreTecnicoAsignado", "Sin asignar")),
                        AreaTecnica = MapArea(GetString(reader, "NombreAreaTecnica")),
                        NombreAreaTecnica = GetString(reader, "NombreAreaTecnica"),
                        DiasAbierto = GetCeilingDays(horasAbiertas),
                        TiempoResolucion = fechaFin.HasValue ? horasAbiertas : null,
                        TiempoAbiertoHoras = horasAbiertas,
                        Satisfaccion = GetNullableRoundedScore(reader, "PromedioEncuesta"),
                        ActivoAfectado = GetString(reader, "NombreActivo", "No registrado"),
                        Ubicacion = GetString(reader, "UbicacionTexto", "No registrada"),
                        CodigoPatrimonial = GetString(reader, "CodigoPatrimonial"),
                        Marca = GetString(reader, "Marca"),
                        Modelo = GetString(reader, "Modelo"),
                        Escalado = GetBoolean(reader, "FueEscalado"),
                        NivelEscalado = GetBoolean(reader, "FueEscalado") ? "Area tecnica" : null,
                        MotivoEscalado = NullIfWhiteSpace(GetString(reader, "MotivoEscalado")),
                        Retrasado = retrasado,
                        DiasRetraso = retrasado && horasObjetivo.HasValue
                            ? GetCeilingDays(horasAbiertas - horasObjetivo.Value)
                            : 0,
                    });
                }
            }
            finally
            {
                if (shouldClose)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return casos;
        }

        public async Task<IEnumerable<ReporteEncuestaDto>> GetEncuestasAsync()
        {
            var encuestas = new List<ReporteEncuestaDto>();

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = @"
SELECT
    IdEncuesta,
    IdCaso,
    NumeroCaso,
    FechaEncuesta,
    Observaciones,
    NombreUsuarioReporta,
    NombreTecnicoAsignado,
    NombreAreaTecnica,
    NombreTipoCaso,
    NombrePrioridad,
    TiempoResolucionHoras,
    PromedioEncuesta,
    IdDetalleEncuesta,
    TextoPregunta,
    TextoRespuesta,
    ValorNumerico
FROM soporte.vwReporteEncuestasBase
ORDER BY FechaEncuesta DESC, IdEncuesta DESC, IdDetalleEncuesta ASC;";
            command.CommandType = CommandType.Text;

            var shouldClose = _context.Database.GetDbConnection().State != ConnectionState.Open;
            if (shouldClose)
            {
                await _context.Database.OpenConnectionAsync();
            }

            try
            {
                using var reader = await command.ExecuteReaderAsync();

                ReporteEncuestaDto? encuestaActual = null;
                long encuestaActualId = 0;

                while (await reader.ReadAsync())
                {
                    var idEncuesta = GetInt64(reader, "IdEncuesta");

                    if (encuestaActual == null || encuestaActualId != idEncuesta)
                    {
                        if (encuestaActual != null)
                        {
                            encuestas.Add(encuestaActual);
                        }

                        var promedio = GetNullableDouble(reader, "PromedioEncuesta") ?? 0;
                        encuestaActualId = idEncuesta;
                        encuestaActual = new ReporteEncuestaDto
                        {
                            Id = idEncuesta,
                            IdCaso = GetInt64(reader, "IdCaso"),
                            CasoId = GetString(reader, "NumeroCaso"),
                            NombreUsuario = GetString(reader, "NombreUsuarioReporta", "Usuario"),
                            NombreTecnico = GetString(reader, "NombreTecnicoAsignado", "Sin asignar"),
                            TecnicoAsignado = GetString(reader, "NombreTecnicoAsignado", "Sin asignar"),
                            AreaTecnica = MapArea(GetString(reader, "NombreAreaTecnica")),
                            TipoCaso = GetString(reader, "NombreTipoCaso", "General"),
                            Prioridad = MapPrioridad(GetString(reader, "NombrePrioridad")),
                            FechaEncuesta = GetDateTime(reader, "FechaEncuesta"),
                            SatisfaccionGeneral = NormalizeScore(promedio),
                            PromedioRespuestas = Math.Round(promedio, 2),
                            TiempoResolucion = Math.Round(GetNullableDouble(reader, "TiempoResolucionHoras") ?? 0, 1),
                            Observaciones = GetString(reader, "Observaciones", "Sin observaciones registradas."),
                        };
                    }

                    if (encuestaActual == null)
                    {
                        continue;
                    }

                    var pregunta = GetString(reader, "TextoPregunta");
                    if (string.IsNullOrWhiteSpace(pregunta))
                    {
                        continue;
                    }

                    encuestaActual.Respuestas.Add(new ReporteEncuestaRespuestaDto
                    {
                        Pregunta = pregunta,
                        Respuesta = GetString(reader, "TextoRespuesta", "Sin respuesta"),
                        ValorNumerico = NormalizeScore(GetNullableDouble(reader, "ValorNumerico") ?? 0),
                    });
                }

                if (encuestaActual != null)
                {
                    encuestas.Add(encuestaActual);
                }
            }
            finally
            {
                if (shouldClose)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return encuestas;
        }

        private static long GetInt64(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? 0 : Convert.ToInt64(value);
        }

        private static long? GetNullableInt64(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? null : Convert.ToInt64(value);
        }

        private static DateTime GetDateTime(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(value);
        }

        private static DateTime? GetNullableDateTime(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? null : Convert.ToDateTime(value);
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

        private static bool GetBoolean(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value != DBNull.Value && Convert.ToBoolean(value);
        }

        private static double GetDouble(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? 0 : Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        private static double? GetNullableDouble(IDataRecord reader, string column)
        {
            var value = reader[column];
            return value == DBNull.Value ? null : Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        private static double? GetNullableRoundedScore(IDataRecord reader, string column)
        {
            var score = GetNullableDouble(reader, column);
            if (!score.HasValue || score.Value <= 0)
            {
                return null;
            }

            return NormalizeScore(score.Value);
        }

        private static int NormalizeScore(double value)
        {
            if (value <= 0)
            {
                return 0;
            }

            return Math.Clamp((int)Math.Round(value, MidpointRounding.AwayFromZero), 1, 5);
        }

        private static int GetCeilingDays(double hours)
        {
            if (hours <= 0)
            {
                return 0;
            }

            return (int)Math.Ceiling(hours / 24d);
        }

        private static string BuildTitle(string description, long id)
        {
            var text = (description ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return $"Caso {id}";
            }

            return text.Length > 80 ? $"{text[..80]}..." : text;
        }

        private static string MapPrioridad(string nombrePrioridad)
        {
            var prioridad = NormalizeText(nombrePrioridad);
            if (prioridad.Contains("critic"))
            {
                return "Critica";
            }

            if (prioridad.Contains("alta"))
            {
                return "Alta";
            }

            if (prioridad.Contains("media"))
            {
                return "Media";
            }

            if (prioridad.Contains("baja"))
            {
                return "Baja";
            }

            return string.IsNullOrWhiteSpace(nombrePrioridad) ? "Media" : nombrePrioridad.Trim();
        }

        private static string MapEstado(string nombreEstado)
        {
            var estado = NormalizeText(nombreEstado);
            if (estado.Contains("cerrad") || estado.Contains("resuelt"))
            {
                return "Cerrado";
            }

            if (estado.Contains("cancel"))
            {
                return "Cancelado";
            }

            if (estado.Contains("escalad"))
            {
                return "Escalado";
            }

            if (estado.Contains("proceso") || estado.Contains("asignad") || estado.Contains("aceptad"))
            {
                return "En Proceso";
            }

            if (estado.Contains("pendient") || estado.Contains("abiert") || estado.Contains("nuevo"))
            {
                return "Abierto";
            }

            return string.IsNullOrWhiteSpace(nombreEstado) ? "Abierto" : nombreEstado.Trim();
        }

        private static string MapArea(string nombreArea)
        {
            var area = NormalizeText(nombreArea);
            if (area.Contains("hard") || area.Contains("infraestructura") || area.Contains("equipo"))
            {
                return "Hardware";
            }

            if (area.Contains("soft") || area.Contains("sistema") || area.Contains("desarrollo") || area.Contains("aplicacion"))
            {
                return "Software";
            }

            if (area.Contains("red") || area.Contains("telecom"))
            {
                return "Redes";
            }

            if (area.Contains("movil") || area.Contains("mobile") || area.Contains("celular"))
            {
                return "Movil";
            }

            return string.IsNullOrWhiteSpace(nombreArea) ? "General" : nombreArea.Trim();
        }

        private static string NormalizeText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = value.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalized.Length);

            foreach (var character in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(char.ToLowerInvariant(character));
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC).Trim();
        }

        private static string? NullIfWhiteSpace(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
