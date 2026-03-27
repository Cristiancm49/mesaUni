using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Application.Services.Soporte
{
    public class TrazabilidadCasoService : ITrazabilidadCasoService
    {
        private readonly ITrazabilidadCasoRepository _repository;

        public TrazabilidadCasoService(ITrazabilidadCasoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TrazabilidadCasoDto>> GetAllAsync()
        {
            var items = await _repository.GetAllWithRelationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<TrazabilidadCasoDto?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdWithRelationsAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<IEnumerable<TrazabilidadCasoDto>> GetByCasoIdAsync(long idCaso)
        {
            var items = await _repository.GetByCasoIdAsync(idCaso);
            return items.Select(MapToDto);
        }

        public async Task<TrazabilidadCasoDto> CreateAsync(TrazabilidadCasoCreateDto dto)
        {
            var entity = new TrazabilidadCaso
            {
                IdCaso = dto.IdCaso,
                FechaEvento = DateTime.UtcNow,
                IdUsuarioAccion = dto.IdUsuarioAccion,
                TipoEvento = dto.TipoEvento,
                Comentario = dto.Comentario,
                IdEstadoCaso = dto.IdEstadoCaso,
                IdAreaTecnica = dto.IdAreaTecnica,
                IdTecnicoAsignado = dto.IdTecnicoAsignado
            };

            await _repository.AddAsync(entity);
            var created = await _repository.GetByIdWithRelationsAsync(entity.Id);
            return MapToDto(created!);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        private TrazabilidadCasoDto MapToDto(TrazabilidadCaso t)
        {
            return new TrazabilidadCasoDto
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
                IdTecnicoAsignado = t.IdTecnicoAsignado,
                IntervencionesTecnicas = t.IntervencionesTecnicas?.Select(i => new IntervencionTecnicaDto
                {
                    Id = i.Id,
                    IdTrazabilidadCaso = i.IdTrazabilidadCaso,
                    IdTipoTrabajo = i.IdTipoTrabajo,
                    NombreTipoTrabajo = i.TipoTrabajo?.NombreTipoTrabajo,
                    IdEstadoIntervencion = i.IdEstadoIntervencion,
                    NombreEstadoIntervencion = i.EstadoIntervencion?.NombreEstado,
                    FechaInicio = i.FechaInicio,
                    FechaFin = i.FechaFin,
                    Diagnostico = i.Diagnostico,
                    SolucionAplicada = i.SolucionAplicada,
                    IdUsuarioAccion = i.IdUsuarioAccion
                }).ToList() ?? new List<IntervencionTecnicaDto>()
            };
        }
    }
}

