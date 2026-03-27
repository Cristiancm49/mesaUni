using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Application.Services.Soporte
{
    public class DetalleEncuestaService : IDetalleEncuestaService
    {
        private readonly IDetalleEncuestaRepository _repository;

        public DetalleEncuestaService(IDetalleEncuestaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DetalleEncuestaDto>> GetAllAsync()
        {
            var items = await _repository.GetAllWithRelationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<DetalleEncuestaDto?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdWithRelationsAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<IEnumerable<DetalleEncuestaDto>> GetByEncuestaIdAsync(long idEncuesta)
        {
            var items = await _repository.GetByEncuestaIdAsync(idEncuesta);
            return items.Select(MapToDto);
        }

        public async Task<DetalleEncuestaDto> CreateAsync(DetalleEncuestaCreateDto dto)
        {
            var entity = new DetalleEncuesta
            {
                IdEncuesta = 0, // Se debe pasar desde el controller
                IdPregunta = dto.IdPregunta,
                IdRespuesta = dto.IdRespuesta,
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

        private DetalleEncuestaDto MapToDto(DetalleEncuesta d)
        {
            return new DetalleEncuestaDto
            {
                Id = d.Id,
                IdEncuesta = d.IdEncuesta,
                IdPregunta = d.IdPregunta,
                TextoPregunta = d.Pregunta?.TextoPregunta,
                IdRespuesta = d.IdRespuesta,
                TextoRespuesta = d.Respuesta?.TextoRespuesta,
                ValorNumerico = d.Respuesta?.ValorNumerico,
                FechaRegistro = d.FechaRegistro
            };
        }
    }
}











