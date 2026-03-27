using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Application.Services.Catalogo
{
    public class CategoriaActivoService : GenericService<CategoriaActivo, CategoriaActivoDto, CategoriaActivoCreateDto, CategoriaActivoUpdateDto>, ICategoriaActivoService
    {
        private readonly ICategoriaActivoRepository _categoriaRepository;

        public CategoriaActivoService(ICategoriaActivoRepository repository) : base(repository)
        {
            _categoriaRepository = repository;
        }

        protected override long GetEntityId(CategoriaActivo entity) => entity.Id;

        protected override CategoriaActivoDto MapToDto(CategoriaActivo entity)
        {
            return new CategoriaActivoDto
            {
                Id = entity.Id,
                NombreCategoria = entity.NombreCategoria,
                Descripcion = entity.Descripcion,
                IdEstadoGeneral = entity.IdEstadoGeneral,
                NombreEstado = entity.EstadoGeneral?.NombreEstado,
                FechaCreacion = entity.FechaCreacion
            };
        }

        protected override CategoriaActivo MapToEntity(CategoriaActivoCreateDto createDto)
        {
            return new CategoriaActivo
            {
                NombreCategoria = createDto.NombreCategoria,
                Descripcion = createDto.Descripcion,
                IdEstadoGeneral = createDto.IdEstadoGeneral,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };
        }

        protected override void MapUpdateToEntity(CategoriaActivoUpdateDto updateDto, CategoriaActivo entity)
        {
            if (!string.IsNullOrEmpty(updateDto.NombreCategoria))
                entity.NombreCategoria = updateDto.NombreCategoria;
            
            if (updateDto.Descripcion != null)
                entity.Descripcion = updateDto.Descripcion;
            
            if (updateDto.IdEstadoGeneral.HasValue)
                entity.IdEstadoGeneral = updateDto.IdEstadoGeneral.Value;
        }

        public override async Task<ApiResponseDto<IEnumerable<CategoriaActivoDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _categoriaRepository.GetAllWithRelationsAsync();
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<CategoriaActivoDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<CategoriaActivoDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<CategoriaActivoDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _categoriaRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<CategoriaActivoDto>
                    {
                        Success = false,
                        Message = $"Categoría con ID {id} no encontrada",
                        Data = null
                    };
                }
                return new ApiResponseDto<CategoriaActivoDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<CategoriaActivoDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<CategoriaActivoDto>> CreateAsync(CategoriaActivoCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _categoriaRepository.AddAsync(entity);
                var entityWithRelations = await _categoriaRepository.GetByIdWithRelationsAsync(created.Id);
                return new ApiResponseDto<CategoriaActivoDto>
                {
                    Success = true,
                    Message = "Categoría creada exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<CategoriaActivoDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<CategoriaActivoDto>> UpdateAsync(long id, CategoriaActivoUpdateDto updateDto)
        {
            try
            {
                var entity = await _categoriaRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<CategoriaActivoDto>
                    {
                        Success = false,
                        Message = $"Categoría con ID {id} no encontrada",
                        Data = null
                    };
                }
                MapUpdateToEntity(updateDto, entity);
                await _categoriaRepository.UpdateAsync(entity);
                var entityWithRelations = await _categoriaRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<CategoriaActivoDto>
                {
                    Success = true,
                    Message = "Categoría actualizada exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<CategoriaActivoDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}












