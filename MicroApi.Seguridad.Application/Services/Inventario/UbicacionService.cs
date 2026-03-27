using MicroApi.Seguridad.Domain.DTOs.Inventario;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Application.Services.Inventario
{
    public class UbicacionService : IUbicacionService
    {
        private readonly IUbicacionRepository _repository;

        public UbicacionService(IUbicacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UbicacionDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllWithSedeAsync();
            return entities.Select(MapToDto);
        }

        public async Task<UbicacionDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdWithSedeAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<UbicacionDto> CreateAsync(UbicacionCreateDto dto)
        {
            var entity = new Ubicacion
            {
                IdSede = dto.IdSede,
                Bloque = dto.Bloque,
                Piso = dto.Piso,
                Sala = dto.Sala,
                Descripcion = dto.Descripcion,
                IdUsuarioCreacion = dto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };

            var created = await _repository.AddAsync(entity);
            var withSede = await _repository.GetByIdWithSedeAsync(created.Id);
            return MapToDto(withSede!);
        }

        public async Task<UbicacionDto?> UpdateAsync(long id, UbicacionUpdateDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            if (dto.IdSede.HasValue)
                entity.IdSede = dto.IdSede.Value;
            if (dto.Bloque != null)
                entity.Bloque = dto.Bloque;
            if (dto.Piso != null)
                entity.Piso = dto.Piso;
            if (dto.Sala != null)
                entity.Sala = dto.Sala;
            if (dto.Descripcion != null)
                entity.Descripcion = dto.Descripcion;

            await _repository.UpdateAsync(entity);
            
            var updated = await _repository.GetByIdWithSedeAsync(id);
            return updated != null ? MapToDto(updated) : null;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static UbicacionDto MapToDto(Ubicacion entity)
        {
            return new UbicacionDto
            {
                Id = entity.Id,
                IdSede = entity.IdSede,
                NombreSede = entity.Sede?.NombreSede ?? "",
                Bloque = entity.Bloque,
                Piso = entity.Piso,
                Sala = entity.Sala,
                Descripcion = entity.Descripcion,
                FechaCreacion = entity.FechaCreacion
            };
        }
    }
}
