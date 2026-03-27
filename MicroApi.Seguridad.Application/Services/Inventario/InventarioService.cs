using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Inventario;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using InventarioEntity = MicroApi.Seguridad.Domain.Models.Inventario.Inventario;

namespace MicroApi.Seguridad.Application.Services.Inventario
{
    public class InventarioService : GenericService<InventarioEntity, InventarioDto, InventarioCreateDto, InventarioUpdateDto>, IInventarioService
    {
        private readonly IInventarioRepository _inventarioRepository;

        public InventarioService(IInventarioRepository repository) : base(repository)
        {
            _inventarioRepository = repository;
        }

        protected override long GetEntityId(InventarioEntity entity) => entity.Id;

        protected override InventarioDto MapToDto(InventarioEntity entity)
        {
            return new InventarioDto
            {
                Id = entity.Id,
                NombreInventario = entity.NombreInventario,
                Descripcion = entity.Descripcion,
                IdEstadoGeneral = entity.IdEstadoGeneral,
                NombreEstado = entity.EstadoGeneral?.NombreEstado,
                IdResponsableInventario = entity.IdResponsableInventario,
                NombreResponsable = entity.Responsable?.NombreCompleto,
                FechaCreacion = entity.FechaCreacion
            };
        }

        protected override InventarioEntity MapToEntity(InventarioCreateDto createDto)
        {
            return new InventarioEntity
            {
                NombreInventario = createDto.NombreInventario,
                Descripcion = createDto.Descripcion,
                IdEstadoGeneral = createDto.IdEstadoGeneral,
                IdResponsableInventario = createDto.IdResponsableInventario,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };
        }

        protected override void MapUpdateToEntity(InventarioUpdateDto updateDto, InventarioEntity entity)
        {
            if (!string.IsNullOrEmpty(updateDto.NombreInventario))
                entity.NombreInventario = updateDto.NombreInventario;
            
            if (updateDto.Descripcion != null)
                entity.Descripcion = updateDto.Descripcion;
            
            if (updateDto.IdEstadoGeneral.HasValue)
                entity.IdEstadoGeneral = updateDto.IdEstadoGeneral.Value;
            
            if (updateDto.IdResponsableInventario.HasValue)
                entity.IdResponsableInventario = updateDto.IdResponsableInventario.Value;
        }

        public override async Task<ApiResponseDto<IEnumerable<InventarioDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _inventarioRepository.GetAllWithRelationsAsync();
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<InventarioDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<InventarioDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<InventarioDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _inventarioRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<InventarioDto>
                    {
                        Success = false,
                        Message = $"Inventario con ID {id} no encontrado",
                        Data = null
                    };
                }
                return new ApiResponseDto<InventarioDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<InventarioDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<InventarioDto>> CreateAsync(InventarioCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _inventarioRepository.AddAsync(entity);
                var entityWithRelations = await _inventarioRepository.GetByIdWithRelationsAsync(created.Id);
                return new ApiResponseDto<InventarioDto>
                {
                    Success = true,
                    Message = "Inventario creado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<InventarioDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<InventarioDto>> UpdateAsync(long id, InventarioUpdateDto updateDto)
        {
            try
            {
                var entity = await _inventarioRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<InventarioDto>
                    {
                        Success = false,
                        Message = $"Inventario con ID {id} no encontrado",
                        Data = null
                    };
                }
                MapUpdateToEntity(updateDto, entity);
                await _inventarioRepository.UpdateAsync(entity);
                var entityWithRelations = await _inventarioRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<InventarioDto>
                {
                    Success = true,
                    Message = "Inventario actualizado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<InventarioDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}

