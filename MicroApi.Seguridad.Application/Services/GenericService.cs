using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Interfaces;

namespace MicroApi.Seguridad.Application.Services
{
    public abstract class GenericService<TEntity, TDto, TCreateDto, TUpdateDto> 
        : IGenericService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;

        protected GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        protected abstract TDto MapToDto(TEntity entity);
        protected abstract TEntity MapToEntity(TCreateDto createDto);
        protected abstract void MapUpdateToEntity(TUpdateDto updateDto, TEntity entity);
        protected abstract long GetEntityId(TEntity entity);

        public virtual async Task<ApiResponseDto<IEnumerable<TDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                var dtos = entities.Select(MapToDto);

                return new ApiResponseDto<IEnumerable<TDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<TDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public virtual async Task<ApiResponseDto<TDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                {
                    return new ApiResponseDto<TDto>
                    {
                        Success = false,
                        Message = $"Registro con ID {id} no encontrado",
                        Data = null
                    };
                }

                return new ApiResponseDto<TDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public virtual async Task<ApiResponseDto<TDto>> CreateAsync(TCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _repository.AddAsync(entity);

                return new ApiResponseDto<TDto>
                {
                    Success = true,
                    Message = "Registro creado",
                    Data = MapToDto(created)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public virtual async Task<ApiResponseDto<TDto>> UpdateAsync(long id, TUpdateDto updateDto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                {
                    return new ApiResponseDto<TDto>
                    {
                        Success = false,
                        Message = $"Registro con ID {id} no encontrado",
                        Data = null
                    };
                }

                MapUpdateToEntity(updateDto, entity);
                var updated = await _repository.UpdateAsync(entity);

                return new ApiResponseDto<TDto>
                {
                    Success = true,
                    Message = "Registro actualizado",
                    Data = MapToDto(updated)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public virtual async Task<ApiResponseDto<int>> CountAsync()
        {
            try
            {
                var count = await _repository.CountAsync();

                return new ApiResponseDto<int>
                {
                    Success = true,
                    Message = "Conteo obtenido",
                    Data = count
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<int>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = 0
                };
            }
        }
    }
}
