using Chaira.MesaServicio.Domain.DTOs.Soporte;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Soporte;
using System.Globalization;
using System.Text;

namespace Chaira.MesaServicio.Application.Services.Soporte
{
    public class CasoService : ICasoService
    {
        private readonly ICasoRepository _repository;
        private readonly IEvidenciaService _evidenciaService;

        public CasoService(ICasoRepository repository, IEvidenciaService evidenciaService)
        {
            _repository = repository;
            _evidenciaService = evidenciaService;
        }

        public async Task<IEnumerable<CasoDto>> GetAllAsync()
        {
            var casos = await _repository.GetAllAsync();
            var mapped = casos.Select(MapToDto).ToList();
            await AttachEvidenciasAsync(mapped);
            return mapped;
        }

        public async Task<CasoDto?> GetByIdAsync(long id)
        {
            var caso = await _repository.GetByIdAsync(id);
            if (caso == null) return null;

            var mapped = MapToDto(caso);
            await AttachEvidenciasAsync(new List<CasoDto> { mapped });
            return mapped;
        }

        public async Task<CasoDetalleDto?> GetDetalleByIdAsync(long id)
        {
            var caso = await _repository.GetByIdAsync(id);
            if (caso == null) return null;

            var detalle = new CasoDetalleDto
            {
                Id = caso.Id,
                NumeroCaso = caso.NumeroCaso,
                Descripcion = caso.Descripcion,
                IdUsuarioReporta = caso.IdUsuarioReporta,
                NombreUsuarioReporta = caso.UsuarioReporta?.NombreCompleto,
                TelefonoContacto = caso.TelefonoContacto,
                CorreoContacto = caso.CorreoContacto,
                IdEstadoCaso = caso.IdEstadoCaso,
                NombreEstadoCaso = caso.EstadoCaso?.NombreEstadoCaso,
                FechaRegistro = caso.FechaRegistro,
                FechaAceptacion = caso.FechaAceptacion,
                FechaResolucion = caso.FechaResolucion,
                FechaCierre = caso.FechaCierre,
                IdTipoCaso = caso.IdTipoCaso,
                NombreTipoCaso = caso.TipoCaso?.NombreTipoCaso,
                IdActivo = caso.IdActivo,
                NombreActivo = caso.Activo?.NombreActivo,
                CodigoPatrimonial = caso.Activo?.CodigoPatrimonial,
                MarcaActivo = caso.Activo?.Marca,
                ModeloActivo = caso.Activo?.Modelo,
                SerieActivo = caso.Activo?.Serie,
                CategoriaActivo = caso.Activo?.CategoriaActivo?.NombreCategoria,
                EstadoActivo = caso.Activo?.EstadoActivo?.NombreEstado,
                UbicacionActivo = BuildUbicacionActivo(caso),
                IdAreaTecnica = caso.IdAreaTecnica,
                NombreAreaTecnica = caso.AreaTecnica?.NombreAreaTecnica,
                IdPrioridad = caso.IdPrioridad,
                NombrePrioridad = caso.Prioridad?.NombrePrioridad,
                TiempoRespuestaDias = caso.Prioridad?.TiempoRespuestaDias,
                TiempoResolucionDias = caso.Prioridad?.TiempoResolucionDias,
                IdCanalIngreso = caso.IdCanalIngreso,
                NombreCanalIngreso = caso.CanalIngreso?.NombreCanal,
                IdTecnicoAsignado = caso.IdTecnicoAsignado,
                NombreTecnicoAsignado = caso.TecnicoAsignado?.NombreCompleto,
                SlaStatus = GetSlaStatus(
                    caso.EstadoCaso?.NombreEstadoCaso,
                    CalculateDays(caso.FechaRegistro, caso.FechaResolucion ?? caso.FechaCierre),
                    caso.Prioridad?.TiempoResolucionDias,
                    caso.Prioridad?.NombrePrioridad
                ),
                FechaActualizacion = caso.FechaActualizacion,
                IdUsuarioCreacion = caso.IdUsuarioCreacion,
                Trazabilidades = caso.Trazabilidades?.Select(t => new TrazabilidadCasoDto
                {
                    Id = t.Id,
                    IdCaso = t.IdCaso,
                    FechaEvento = t.FechaEvento,
                    IdUsuarioAccion = t.IdUsuarioAccion,
                    TipoEvento = t.TipoEvento,
                    Comentario = t.Comentario,
                    IdEstadoCaso = t.IdEstadoCaso,
                    NombreEstadoCaso = t.EstadoCaso?.NombreEstadoCaso,
                    IdAreaTecnica = t.IdAreaTecnica,
                    NombreAreaTecnica = t.AreaTecnica?.NombreAreaTecnica,
                    IdTecnicoAsignado = t.IdTecnicoAsignado
                }).ToList() ?? new List<TrazabilidadCasoDto>()
            };

            try
            {
                var evidencias = await _evidenciaService.GetByEntidadAsync("soporte", "caso", caso.Id);
                detalle.Evidencias = evidencias.ToList();
            }
            catch
            {
                detalle.Evidencias = new List<EvidenciaDto>();
            }

            return detalle;
        }

        public async Task<IEnumerable<CasoDto>> GetByTecnicoAsync(long idTecnico)
        {
            var casos = await _repository.GetByTecnicoAsync(idTecnico);
            var mapped = casos.Select(MapToDto).ToList();
            await AttachEvidenciasAsync(mapped);
            return mapped;
        }

        public async Task<IEnumerable<CasoDto>> GetByUsuarioReportaAsync(long idUsuarioReporta, MisIncidenciasFiltrosDto? filtros = null)
        {
            var casos = await _repository.GetByUsuarioReportaAsync(idUsuarioReporta);
            var mapped = casos.Select(MapToDto).ToList();
            var filtrados = ApplyMisIncidenciasFilters(mapped, filtros);
            await AttachEvidenciasAsync(filtrados);
            return filtrados;
        }

        public async Task<IEnumerable<CasoDto>> GetByEstadoAsync(long idEstadoCaso)
        {
            var casos = await _repository.GetByEstadoAsync(idEstadoCaso);
            var mapped = casos.Select(MapToDto).ToList();
            await AttachEvidenciasAsync(mapped);
            return mapped;
        }

        public async Task<IEnumerable<CasoDto>> GetByFiltrosAsync(CasoFiltrosDto filtros)
        {
            var casos = await _repository.GetByFiltrosAsync(
                filtros.IdEstadoCaso,
                filtros.IdTecnico,
                filtros.IdAreaTecnica,
                filtros.FechaDesde,
                filtros.FechaHasta
            );
            var mapped = casos.Select(MapToDto).ToList();
            await AttachEvidenciasAsync(mapped);
            return mapped;
        }

        public async Task<CasoDto> CreateAsync(CasoCreateDto dto)
        {
            var caso = new Caso
            {
                Descripcion = dto.Descripcion,
                IdUsuarioReporta = dto.IdUsuarioReporta,
                TelefonoContacto = dto.TelefonoContacto,
                CorreoContacto = dto.CorreoContacto,
                IdEstadoCaso = dto.IdEstadoCaso,
                FechaRegistro = DateTime.UtcNow,
                IdTipoCaso = dto.IdTipoCaso,
                IdActivo = dto.IdActivo,
                IdAreaTecnica = dto.IdAreaTecnica,
                IdPrioridad = dto.IdPrioridad,
                IdCanalIngreso = dto.IdCanalIngreso,
                IdTecnicoAsignado = dto.IdTecnicoAsignado,
                IdUsuarioCreacion = dto.IdUsuarioCreacion
            };

            var created = await _repository.CreateAsync(caso);
            return MapToDto(created);
        }

        public async Task<CasoDto?> UpdateAsync(CasoUpdateDto dto)
        {
            var caso = await _repository.GetByIdAsync(dto.Id);
            if (caso == null) return null;

            if (dto.Descripcion != null) caso.Descripcion = dto.Descripcion;
            if (dto.TelefonoContacto != null) caso.TelefonoContacto = dto.TelefonoContacto;
            if (dto.CorreoContacto != null) caso.CorreoContacto = dto.CorreoContacto;
            if (dto.IdEstadoCaso.HasValue) caso.IdEstadoCaso = dto.IdEstadoCaso.Value;
            if (dto.FechaAceptacion.HasValue) caso.FechaAceptacion = dto.FechaAceptacion;
            if (dto.FechaResolucion.HasValue) caso.FechaResolucion = dto.FechaResolucion;
            if (dto.FechaCierre.HasValue) caso.FechaCierre = dto.FechaCierre;
            if (dto.IdTipoCaso.HasValue) caso.IdTipoCaso = dto.IdTipoCaso.Value;
            if (dto.IdActivo.HasValue) caso.IdActivo = dto.IdActivo;
            if (dto.IdAreaTecnica.HasValue) caso.IdAreaTecnica = dto.IdAreaTecnica;
            if (dto.IdPrioridad.HasValue) caso.IdPrioridad = dto.IdPrioridad.Value;
            if (dto.IdCanalIngreso.HasValue) caso.IdCanalIngreso = dto.IdCanalIngreso.Value;
            if (dto.IdTecnicoAsignado.HasValue) caso.IdTecnicoAsignado = dto.IdTecnicoAsignado;

            var updated = await _repository.UpdateAsync(caso);
            return MapToDto(updated);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        private static List<CasoDto> ApplyMisIncidenciasFilters(List<CasoDto> casos, MisIncidenciasFiltrosDto? filtros)
        {
            if (casos.Count == 0 || filtros == null)
            {
                return casos;
            }

            IEnumerable<CasoDto> query = casos;

            var estadoGeneral = NormalizeText(filtros.EstadoGeneral);
            if (estadoGeneral == "activos")
            {
                query = query.Where(caso => MapEstadoCaso(caso.NombreEstadoCaso) == "Activo");
            }
            else if (estadoGeneral == "resueltos")
            {
                query = query.Where(caso => MapEstadoCaso(caso.NombreEstadoCaso) == "Resuelto");
            }

            var busqueda = NormalizeText(filtros.Busqueda);
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                query = query.Where(caso =>
                {
                    var numeroCaso = NormalizeText(caso.NumeroCaso ?? BuildNumeroCaso(caso.Id, caso.FechaRegistro));
                    var solicitante = NormalizeText(caso.NombreUsuarioReporta);
                    var descripcion = NormalizeText(caso.Descripcion);
                    var areaTecnica = NormalizeText(caso.NombreAreaTecnica);
                    var tipoCaso = NormalizeText(caso.NombreTipoCaso);
                    var tecnico = NormalizeText(caso.NombreTecnicoAsignado);

                    return numeroCaso.Contains(busqueda) ||
                           solicitante.Contains(busqueda) ||
                           descripcion.Contains(busqueda) ||
                           areaTecnica.Contains(busqueda) ||
                           tipoCaso.Contains(busqueda) ||
                           tecnico.Contains(busqueda);
                });
            }

            var prioridad = NormalizeText(filtros.Prioridad);
            if (!string.IsNullOrWhiteSpace(prioridad))
            {
                query = query.Where(caso => NormalizeText(MapPrioridad(caso.NombrePrioridad)) == prioridad);
            }

            var area = NormalizeText(filtros.AreaTecnica);
            if (!string.IsNullOrWhiteSpace(area))
            {
                query = query.Where(caso => NormalizeText(caso.NombreAreaTecnica) == area);
            }

            var estadoEspecifico = NormalizeText(filtros.EstadoEspecifico);
            if (!string.IsNullOrWhiteSpace(estadoEspecifico))
            {
                query = query.Where(caso => MatchesSpecificState(caso, estadoEspecifico));
            }

            var tecnicoFiltro = NormalizeText(filtros.Tecnico);
            if (!string.IsNullOrWhiteSpace(tecnicoFiltro))
            {
                query = query.Where(caso => NormalizeText(caso.NombreTecnicoAsignado) == tecnicoFiltro);
            }

            var slaStatus = NormalizeText(filtros.SlaStatus);
            if (!string.IsNullOrWhiteSpace(slaStatus))
            {
                query = query.Where(caso => NormalizeText(caso.SlaStatus) == slaStatus);
            }

            if (filtros.FechaDesde.HasValue)
            {
                var fechaDesde = filtros.FechaDesde.Value.Date;
                query = query.Where(caso => caso.FechaRegistro.Date >= fechaDesde);
            }

            if (filtros.FechaHasta.HasValue)
            {
                var fechaHasta = filtros.FechaHasta.Value.Date;
                query = query.Where(caso => caso.FechaRegistro.Date <= fechaHasta);
            }

            return query.ToList();
        }

        private static string BuildNumeroCaso(long id, DateTime fechaRegistro)
            => $"CASO-{fechaRegistro.Year}-{id}";

        private static int CalculateDays(DateTime fromDate, DateTime? toDate)
        {
            var to = toDate ?? DateTime.UtcNow;
            var diff = (int)Math.Ceiling((to - fromDate).TotalDays);
            return Math.Max(diff, 0);
        }

        private static string MapEstadoCaso(string? nombreEstadoCaso)
        {
            var estadoNormalizado = NormalizeText(nombreEstadoCaso);

            if (estadoNormalizado.Contains("resuelt") || estadoNormalizado.Contains("cerrad")) return "Resuelto";
            if (estadoNormalizado.Contains("proceso") || estadoNormalizado.Contains("progreso")) return "En Proceso";
            if (estadoNormalizado.Contains("pendient")) return "Pendiente";
            return "Activo";
        }

        private static string MapPrioridad(string? nombrePrioridad)
        {
            var prioridadNormalizada = NormalizeText(nombrePrioridad);

            if (prioridadNormalizada.Contains("alta")) return "Alta";
            if (prioridadNormalizada.Contains("media")) return "Media";
            if (prioridadNormalizada.Contains("baja")) return "Baja";
            if (prioridadNormalizada.Contains("critic")) return "Critica";

            return string.IsNullOrWhiteSpace(nombrePrioridad) ? "Media" : nombrePrioridad;
        }

        private static bool MatchesSpecificState(CasoDto caso, string estadoFiltro)
        {
            var estadoCatalogo = NormalizeText(caso.NombreEstadoCaso);
            var estadoMapeado = NormalizeText(MapEstadoCaso(caso.NombreEstadoCaso));

            return estadoCatalogo == estadoFiltro ||
                   estadoMapeado == estadoFiltro;
        }

        private static int ResolveSlaLimitDays(int? tiempoResolucionDias, string? nombrePrioridad)
        {
            if (tiempoResolucionDias.HasValue && tiempoResolucionDias.Value > 0)
            {
                return tiempoResolucionDias.Value;
            }

            return MapPrioridad(nombrePrioridad) switch
            {
                "Critica" => 1,
                "Alta" => 2,
                "Media" => 4,
                "Baja" => 7,
                _ => 4,
            };
        }

        private static string GetSlaStatus(string? nombreEstadoCaso, int diasAbierto, int? tiempoResolucionDias, string? nombrePrioridad)
        {
            var estado = MapEstadoCaso(nombreEstadoCaso);
            if (estado == "Resuelto") return "Cumplido";

            var limite = ResolveSlaLimitDays(tiempoResolucionDias, nombrePrioridad);

            if (diasAbierto > limite) return "Vencido";
            if (diasAbierto == limite) return "En Riesgo";
            return "En Tiempo";
        }

        private static string NormalizeText(string? value)
        {
            var normalized = RemoveDiacritics(value).ToLowerInvariant().Trim();
            return normalized;
        }

        private static string RemoveDiacritics(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = value.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalized.Length);

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        private async Task AttachEvidenciasAsync(List<CasoDto> casos)
        {
            if (casos.Count == 0)
            {
                return;
            }

            try
            {
                var ids = casos.Select(x => x.Id).Where(x => x > 0).Distinct().ToList();
                if (ids.Count == 0)
                {
                    return;
                }

                var evidencias = await _evidenciaService.GetByEntidadesAsync("soporte", "caso", ids);
                var evidenciasMap = evidencias.ToDictionary(x => x.EntidadId, x => x.Evidencias);

                foreach (var caso in casos)
                {
                    caso.Evidencias = evidenciasMap.TryGetValue(caso.Id, out var list)
                        ? list
                        : new List<EvidenciaDto>();
                }
            }
            catch
            {
                foreach (var caso in casos)
                {
                    caso.Evidencias = new List<EvidenciaDto>();
                }
            }
        }

        private CasoDto MapToDto(Caso caso)
        {
            return new CasoDto
            {
                Id = caso.Id,
                NumeroCaso = caso.NumeroCaso,
                Descripcion = caso.Descripcion,
                IdUsuarioReporta = caso.IdUsuarioReporta,
                NombreUsuarioReporta = caso.UsuarioReporta?.NombreCompleto,
                TelefonoContacto = caso.TelefonoContacto,
                CorreoContacto = caso.CorreoContacto,
                IdEstadoCaso = caso.IdEstadoCaso,
                NombreEstadoCaso = caso.EstadoCaso?.NombreEstadoCaso,
                FechaRegistro = caso.FechaRegistro,
                FechaAceptacion = caso.FechaAceptacion,
                FechaResolucion = caso.FechaResolucion,
                FechaCierre = caso.FechaCierre,
                IdTipoCaso = caso.IdTipoCaso,
                NombreTipoCaso = caso.TipoCaso?.NombreTipoCaso,
                IdActivo = caso.IdActivo,
                NombreActivo = caso.Activo?.NombreActivo,
                CodigoPatrimonial = caso.Activo?.CodigoPatrimonial,
                MarcaActivo = caso.Activo?.Marca,
                ModeloActivo = caso.Activo?.Modelo,
                SerieActivo = caso.Activo?.Serie,
                CategoriaActivo = caso.Activo?.CategoriaActivo?.NombreCategoria,
                EstadoActivo = caso.Activo?.EstadoActivo?.NombreEstado,
                UbicacionActivo = BuildUbicacionActivo(caso),
                IdAreaTecnica = caso.IdAreaTecnica,
                NombreAreaTecnica = caso.AreaTecnica?.NombreAreaTecnica,
                IdPrioridad = caso.IdPrioridad,
                NombrePrioridad = caso.Prioridad?.NombrePrioridad,
                TiempoRespuestaDias = caso.Prioridad?.TiempoRespuestaDias,
                TiempoResolucionDias = caso.Prioridad?.TiempoResolucionDias,
                IdCanalIngreso = caso.IdCanalIngreso,
                NombreCanalIngreso = caso.CanalIngreso?.NombreCanal,
                IdTecnicoAsignado = caso.IdTecnicoAsignado,
                NombreTecnicoAsignado = caso.TecnicoAsignado?.NombreCompleto,
                SlaStatus = GetSlaStatus(
                    caso.EstadoCaso?.NombreEstadoCaso,
                    CalculateDays(caso.FechaRegistro, caso.FechaResolucion ?? caso.FechaCierre),
                    caso.Prioridad?.TiempoResolucionDias,
                    caso.Prioridad?.NombrePrioridad
                ),
                FechaActualizacion = caso.FechaActualizacion,
                IdUsuarioCreacion = caso.IdUsuarioCreacion
            };
        }

        private static string? BuildUbicacionActivo(Caso caso)
        {
            if (caso.Activo?.Ubicacion == null)
            {
                return null;
            }

            var partes = new List<string>();

            if (!string.IsNullOrWhiteSpace(caso.Activo.Ubicacion.Sede?.NombreSede))
            {
                partes.Add(caso.Activo.Ubicacion.Sede.NombreSede);
            }

            if (!string.IsNullOrWhiteSpace(caso.Activo.Ubicacion.Bloque))
            {
                partes.Add($"Bloque {caso.Activo.Ubicacion.Bloque}");
            }

            if (!string.IsNullOrWhiteSpace(caso.Activo.Ubicacion.Piso))
            {
                partes.Add($"Piso {caso.Activo.Ubicacion.Piso}");
            }

            if (!string.IsNullOrWhiteSpace(caso.Activo.Ubicacion.Sala))
            {
                partes.Add(caso.Activo.Ubicacion.Sala);
            }

            if (!string.IsNullOrWhiteSpace(caso.Activo.Ubicacion.Descripcion))
            {
                partes.Add(caso.Activo.Ubicacion.Descripcion);
            }

            return partes.Count > 0 ? string.Join(" - ", partes) : null;
        }
    }
}

