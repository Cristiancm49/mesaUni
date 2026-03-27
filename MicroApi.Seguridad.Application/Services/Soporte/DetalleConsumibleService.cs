using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Application.Services.Soporte
{
    public class DetalleConsumibleService : IDetalleConsumibleService
    {
        private readonly IDetalleConsumibleRepository _repository;

        public DetalleConsumibleService(IDetalleConsumibleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DetalleConsumibleDto>> GetAllAsync()
        {
            var items = await _repository.GetAllWithRelationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<DetalleConsumibleDto?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdWithRelationsAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<IEnumerable<DetalleConsumibleDto>> GetByIntervencionIdAsync(long idIntervencion)
        {
            var items = await _repository.GetByIntervencionIdAsync(idIntervencion);
            return items.Select(MapToDto);
        }

        public async Task<DetalleConsumibleDto> CreateAsync(DetalleConsumibleCreateDto dto)
        {
            var entity = new DetalleConsumible
            {
                IdIntervencionTecnica = dto.IdIntervencionTecnica,
                IdConsumible = dto.IdConsumible,
                Cantidad = dto.Cantidad,
                DescripcionUso = dto.DescripcionUso,
                FechaRegistro = DateTime.UtcNow,
                IdUsuarioCreacion = dto.IdUsuarioCreacion
            };

            await _repository.AddAsync(entity);
            var created = await _repository.GetByIdWithRelationsAsync(entity.Id);
            return MapToDto(created!);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        private DetalleConsumibleDto MapToDto(DetalleConsumible d)
        {
            return new DetalleConsumibleDto
            {
                Id = d.Id,
                IdIntervencionTecnica = d.IdIntervencionTecnica,
                IdConsumible = d.IdConsumible,
                NombreConsumible = d.Consumible?.NombreConsumible,
                Cantidad = d.Cantidad,
                DescripcionUso = d.DescripcionUso,
                FechaRegistro = d.FechaRegistro,
                IdUsuarioCreacion = d.IdUsuarioCreacion
            };
        }
    }
}











