using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Application.Services.Soporte
{
    public class RevisionAdmiService : IRevisionAdmiService
    {
        private readonly IRevisionAdmiRepository _repository;

        public RevisionAdmiService(IRevisionAdmiRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RevisionAdmiDto>> GetAllAsync()
        {
            var items = await _repository.GetAllWithRelationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<RevisionAdmiDto?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdWithRelationsAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<RevisionAdmiDto?> GetByIntervencionIdAsync(long idIntervencion)
        {
            var item = await _repository.GetByIntervencionIdAsync(idIntervencion);
            return item == null ? null : MapToDto(item);
        }

        public async Task<RevisionAdmiDto> CreateAsync(RevisionAdmiCreateDto dto)
        {
            var entity = new RevisionAdmi
            {
                IdIntervencionTecnica = dto.IdIntervencionTecnica,
                Aprobado = dto.Aprobado,
                ObservacionRevision = dto.ObservacionRevision,
                FechaRegistro = DateTime.UtcNow,
                IdUsuarioCreacion = dto.IdUsuarioCreacion
            };

            await _repository.AddAsync(entity);
            var created = await _repository.GetByIdWithRelationsAsync(entity.Id);
            return MapToDto(created!);
        }

        public async Task<RevisionAdmiDto?> UpdateAsync(RevisionAdmiUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(dto.Id);
            if (entity == null) return null;

            if (dto.Aprobado.HasValue) entity.Aprobado = dto.Aprobado.Value;
            if (dto.ObservacionRevision != null) entity.ObservacionRevision = dto.ObservacionRevision;

            await _repository.UpdateAsync(entity);
            var updated = await _repository.GetByIdWithRelationsAsync(entity.Id);
            return MapToDto(updated!);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        private RevisionAdmiDto MapToDto(RevisionAdmi r)
        {
            return new RevisionAdmiDto
            {
                Id = r.Id,
                IdIntervencionTecnica = r.IdIntervencionTecnica,
                Aprobado = r.Aprobado,
                ObservacionRevision = r.ObservacionRevision,
                FechaRegistro = r.FechaRegistro,
                IdUsuarioCreacion = r.IdUsuarioCreacion
            };
        }
    }
}











