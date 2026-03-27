using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Application.Services.Soporte
{
    public class IntervencionTecnicaService : IIntervencionTecnicaService
    {
        private readonly IIntervencionTecnicaRepository _repository;

        public IntervencionTecnicaService(IIntervencionTecnicaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IntervencionTecnicaDto>> GetAllAsync()
        {
            var items = await _repository.GetAllWithRelationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<IntervencionTecnicaDto?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdWithRelationsAsync(id);
            return item == null ? null : MapToDtoDetalle(item);
        }

        public async Task<IEnumerable<IntervencionTecnicaDto>> GetByTrazabilidadIdAsync(long idTrazabilidad)
        {
            var items = await _repository.GetByTrazabilidadIdAsync(idTrazabilidad);
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<IntervencionTecnicaDto>> GetByCasoIdAsync(long idCaso)
        {
            var items = await _repository.GetByCasoIdAsync(idCaso);
            return items.Select(MapToDtoDetalle);
        }

        public async Task<IntervencionTecnicaDto> CreateAsync(IntervencionTecnicaCreateDto dto)
        {
            var entity = new IntervencionTecnica
            {
                IdTrazabilidadCaso = dto.IdTrazabilidadCaso,
                IdTipoTrabajo = dto.IdTipoTrabajo,
                IdEstadoIntervencion = dto.IdEstadoIntervencion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Diagnostico = dto.Diagnostico,
                SolucionAplicada = dto.SolucionAplicada,
                IdUsuarioAccion = dto.IdUsuarioAccion
            };

            await _repository.AddAsync(entity);
            var created = await _repository.GetByIdWithRelationsAsync(entity.Id);
            return MapToDto(created!);
        }

        public async Task<IntervencionTecnicaDto?> UpdateAsync(IntervencionTecnicaUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity == null) return null;

            if (dto.IdTipoTrabajo.HasValue) entity.IdTipoTrabajo = dto.IdTipoTrabajo.Value;
            if (dto.IdEstadoIntervencion.HasValue) entity.IdEstadoIntervencion = dto.IdEstadoIntervencion.Value;
            if (dto.FechaFin.HasValue) entity.FechaFin = dto.FechaFin;
            if (dto.Diagnostico != null) entity.Diagnostico = dto.Diagnostico;
            if (dto.SolucionAplicada != null) entity.SolucionAplicada = dto.SolucionAplicada;

            await _repository.UpdateAsync(entity);
            var updated = await _repository.GetByIdWithRelationsAsync(entity.Id);
            return MapToDto(updated!);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        private IntervencionTecnicaDto MapToDto(IntervencionTecnica i)
        {
            return new IntervencionTecnicaDto
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
            };
        }

        private IntervencionTecnicaDto MapToDtoDetalle(IntervencionTecnica i)
        {
            var dto = MapToDto(i);
            dto.CambiosComponentes = i.CambiosComponentes?.Select(c => new DetalleCambioComponentesDto
            {
                Id = c.Id,
                IdIntervencionTecnica = c.IdIntervencionTecnica,
                IdComponente = c.IdComponente,
                NombreComponente = c.Componente?.NombreComponente,
                Cantidad = c.Cantidad,
                TipoCambio = c.TipoCambio,
                DescripcionCambio = c.DescripcionCambio,
                FechaRegistro = c.FechaRegistro,
                IdUsuarioCreacion = c.IdUsuarioCreacion
            }).ToList() ?? new List<DetalleCambioComponentesDto>();

            dto.DetallesConsumibles = i.DetallesConsumibles?.Select(d => new DetalleConsumibleDto
            {
                Id = d.Id,
                IdIntervencionTecnica = d.IdIntervencionTecnica,
                IdConsumible = d.IdConsumible,
                NombreConsumible = d.Consumible?.NombreConsumible,
                Cantidad = d.Cantidad,
                DescripcionUso = d.DescripcionUso,
                FechaRegistro = d.FechaRegistro,
                IdUsuarioCreacion = d.IdUsuarioCreacion
            }).ToList() ?? new List<DetalleConsumibleDto>();

            var ultimaRevision = i.RevisionesAdmi?
                .OrderByDescending(r => r.FechaRegistro)
                .FirstOrDefault();

            if (ultimaRevision != null)
            {
                dto.RevisionAdmi = new RevisionAdmiDto
                {
                    Id = ultimaRevision.Id,
                    IdIntervencionTecnica = ultimaRevision.IdIntervencionTecnica,
                    Aprobado = ultimaRevision.Aprobado,
                    ObservacionRevision = ultimaRevision.ObservacionRevision,
                    FechaRegistro = ultimaRevision.FechaRegistro,
                    IdUsuarioCreacion = ultimaRevision.IdUsuarioCreacion
                };
            }

            return dto;
        }
    }
}
