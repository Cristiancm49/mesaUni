using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Application.Services.Soporte
{
    public class EncuestaCalidadService : IEncuestaCalidadService
    {
        private readonly IEncuestaCalidadRepository _repository;
        private readonly IDetalleEncuestaRepository _detalleRepository;

        public EncuestaCalidadService(
            IEncuestaCalidadRepository repository,
            IDetalleEncuestaRepository detalleRepository)
        {
            _repository = repository;
            _detalleRepository = detalleRepository;
        }

        public async Task<IEnumerable<EncuestaCalidadDto>> GetAllAsync()
        {
            var items = await _repository.GetAllWithRelationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<EncuestaCalidadDto?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdWithRelationsAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<IEnumerable<EncuestaCalidadDto>> GetByCasoIdAsync(long idCaso)
        {
            var items = await _repository.GetByCasoIdAsync(idCaso);
            return items.Select(MapToDto);
        }

        public async Task<EncuestaCalidadDto> CreateAsync(EncuestaCalidadCreateDto dto)
        {
            var entity = new EncuestaCalidad
            {
                IdCaso = dto.IdCaso,
                FechaEncuesta = DateTime.UtcNow,
                Observaciones = dto.Observaciones,
                IdUsuarioCreacion = dto.IdUsuarioCreacion
            };

            await _repository.AddAsync(entity);

            foreach (var detalle in dto.Detalles)
            {
                var detalleEntity = new DetalleEncuesta
                {
                    IdEncuesta = entity.Id,
                    IdPregunta = detalle.IdPregunta,
                    IdRespuesta = detalle.IdRespuesta,
                    FechaRegistro = DateTime.UtcNow,
                    IdUsuarioCreacion = detalle.IdUsuarioCreacion
                };
                await _detalleRepository.AddAsync(detalleEntity);
            }

            var created = await _repository.GetByIdWithRelationsAsync(entity.Id);
            return MapToDto(created!);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        private EncuestaCalidadDto MapToDto(EncuestaCalidad e)
        {
            return new EncuestaCalidadDto
            {
                Id = e.Id,
                IdCaso = e.IdCaso,
                FechaEncuesta = e.FechaEncuesta,
                Observaciones = e.Observaciones,
                IdUsuarioCreacion = e.IdUsuarioCreacion,
                Detalles = e.Detalles?.Select(d => new DetalleEncuestaDto
                {
                    Id = d.Id,
                    IdEncuesta = d.IdEncuesta,
                    IdPregunta = d.IdPregunta,
                    TextoPregunta = d.Pregunta?.TextoPregunta,
                    IdRespuesta = d.IdRespuesta,
                    TextoRespuesta = d.Respuesta?.TextoRespuesta,
                    ValorNumerico = d.Respuesta?.ValorNumerico,
                    FechaRegistro = d.FechaRegistro
                }).ToList() ?? new List<DetalleEncuestaDto>()
            };
        }
    }
}
