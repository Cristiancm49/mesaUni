using Chaira.MesaServicio.Domain.DTOs.Catalogo;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Application.Services.Catalogo
{
    public class TipoConsumibleService : GenericService<TipoConsumible, TipoConsumibleDto, TipoConsumibleCreateDto, TipoConsumibleUpdateDto>, ITipoConsumibleService
    {
        private readonly ITipoConsumibleRepository _tipoConsumibleRepository;

        public TipoConsumibleService(ITipoConsumibleRepository repository) : base(repository)
        {
            _tipoConsumibleRepository = repository;
        }

        protected override long GetEntityId(TipoConsumible entity) => entity.Id;

        protected override TipoConsumibleDto MapToDto(TipoConsumible entity)
        {
            return new TipoConsumibleDto
            {
                Id = entity.Id,
                NombreTipo = entity.NombreTipo,
                Descripcion = entity.Descripcion,
                IdEstadoGeneral = entity.IdEstadoGeneral,
                NombreEstado = entity.EstadoGeneral?.NombreEstado,
                FechaCreacion = entity.FechaCreacion
            };
        }

        protected override TipoConsumible MapToEntity(TipoConsumibleCreateDto createDto)
        {
            return new TipoConsumible
            {
                NombreTipo = createDto.NombreTipo,
                Descripcion = createDto.Descripcion,
                IdEstadoGeneral = createDto.IdEstadoGeneral,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };
        }

        protected override void MapUpdateToEntity(TipoConsumibleUpdateDto updateDto, TipoConsumible entity)
        {
            if (!string.IsNullOrEmpty(updateDto.NombreTipo))
                entity.NombreTipo = updateDto.NombreTipo;
            
            if (updateDto.Descripcion != null)
                entity.Descripcion = updateDto.Descripcion;
            
            if (updateDto.IdEstadoGeneral.HasValue)
                entity.IdEstadoGeneral = updateDto.IdEstadoGeneral.Value;
        }

        public override async Task<ApiResponseDto<IEnumerable<TipoConsumibleDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _tipoConsumibleRepository.GetAllWithRelationsAsync();
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<TipoConsumibleDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<TipoConsumibleDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<TipoConsumibleDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _tipoConsumibleRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<TipoConsumibleDto>
                    {
                        Success = false,
                        Message = $"Tipo de consumible con ID {id} no encontrado",
                        Data = null
                    };
                }
                return new ApiResponseDto<TipoConsumibleDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoConsumibleDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<TipoConsumibleDto>> CreateAsync(TipoConsumibleCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _tipoConsumibleRepository.AddAsync(entity);
                var entityWithRelations = await _tipoConsumibleRepository.GetByIdWithRelationsAsync(created.Id);
                return new ApiResponseDto<TipoConsumibleDto>
                {
                    Success = true,
                    Message = "Tipo de consumible creado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoConsumibleDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<TipoConsumibleDto>> UpdateAsync(long id, TipoConsumibleUpdateDto updateDto)
        {
            try
            {
                var entity = await _tipoConsumibleRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<TipoConsumibleDto>
                    {
                        Success = false,
                        Message = $"Tipo de consumible con ID {id} no encontrado",
                        Data = null
                    };
                }
                MapUpdateToEntity(updateDto, entity);
                await _tipoConsumibleRepository.UpdateAsync(entity);
                var entityWithRelations = await _tipoConsumibleRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<TipoConsumibleDto>
                {
                    Success = true,
                    Message = "Tipo de consumible actualizado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoConsumibleDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}













