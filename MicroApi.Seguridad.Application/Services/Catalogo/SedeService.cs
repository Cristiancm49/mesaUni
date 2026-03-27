using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Application.Services.Catalogo
{
    public class SedeService : ISedeService
    {
        private readonly ISedeRepository _repository;

        public SedeService(ISedeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SedeDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllWithEstadoAsync();
            return entities.Select(MapToDto);
        }

        public async Task<SedeDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdWithEstadoAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<SedeDto> CreateAsync(SedeCreateDto dto)
        {
            var entity = new Sede
            {
                NombreSede = dto.NombreSede,
                Descripcion = dto.Descripcion,
                IdEstadoGeneral = dto.IdEstadoGeneral,
                IdUsuarioCreacion = dto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };

            var created = await _repository.AddAsync(entity);
            return MapToDto(created);
        }

        public async Task<SedeDto?> UpdateAsync(long id, SedeUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            if (!string.IsNullOrEmpty(dto.NombreSede))
                entity.NombreSede = dto.NombreSede;
            if (dto.Descripcion != null)
                entity.Descripcion = dto.Descripcion;
            if (dto.IdEstadoGeneral.HasValue)
                entity.IdEstadoGeneral = dto.IdEstadoGeneral.Value;

            await _repository.UpdateAsync(entity);
            
            var updated = await _repository.GetByIdWithEstadoAsync(id);
            return updated != null ? MapToDto(updated) : null;
        }

        private static SedeDto MapToDto(Sede entity)
        {
            return new SedeDto
            {
                Id = entity.Id,
                NombreSede = entity.NombreSede,
                Descripcion = entity.Descripcion,
                IdEstadoGeneral = entity.IdEstadoGeneral,
                NombreEstado = entity.EstadoGeneral?.NombreEstado,
                FechaCreacion = entity.FechaCreacion
            };
        }
    }
}










