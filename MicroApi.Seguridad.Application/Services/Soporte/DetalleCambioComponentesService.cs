using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Application.Services.Soporte
{
    public class DetalleCambioComponentesService : IDetalleCambioComponentesService
    {
        private readonly IDetalleCambioComponentesRepository _repository;

        public DetalleCambioComponentesService(IDetalleCambioComponentesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DetalleCambioComponentesDto>> GetAllAsync()
        {
            var items = await _repository.GetAllWithRelationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<DetalleCambioComponentesDto?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdWithRelationsAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<IEnumerable<DetalleCambioComponentesDto>> GetByIntervencionIdAsync(long idIntervencion)
        {
            var items = await _repository.GetByIntervencionIdAsync(idIntervencion);
            return items.Select(MapToDto);
        }

        public async Task<DetalleCambioComponentesDto> CreateAsync(DetalleCambioComponentesCreateDto dto)
        {
            var entity = new DetalleCambioComponentes
            {
                IdIntervencionTecnica = dto.IdIntervencionTecnica,
                IdComponente = dto.IdComponente,
                Cantidad = dto.Cantidad,
                TipoCambio = dto.TipoCambio,
                DescripcionCambio = dto.DescripcionCambio,
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

        private DetalleCambioComponentesDto MapToDto(DetalleCambioComponentes d)
        {
            return new DetalleCambioComponentesDto
            {
                Id = d.Id,
                IdIntervencionTecnica = d.IdIntervencionTecnica,
                IdComponente = d.IdComponente,
                NombreComponente = d.Componente?.NombreComponente,
                Cantidad = d.Cantidad,
                TipoCambio = d.TipoCambio,
                DescripcionCambio = d.DescripcionCambio,
                FechaRegistro = d.FechaRegistro,
                IdUsuarioCreacion = d.IdUsuarioCreacion
            };
        }
    }
}











