using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Application.Services.Inventario
{
    public class ComponenteService : GenericService<Componente, ComponenteDto, ComponenteCreateDto, ComponenteUpdateDto>, IComponenteService
    {
        private readonly IComponenteRepository _componenteRepository;

        public ComponenteService(IComponenteRepository repository) : base(repository)
        {
            _componenteRepository = repository;
        }

        protected override long GetEntityId(Componente entity) => entity.Id;

        protected override ComponenteDto MapToDto(Componente entity)
        {
            return new ComponenteDto
            {
                Id = entity.Id,
                NombreComponente = entity.NombreComponente,
                Marca = entity.Marca,
                Modelo = entity.Modelo,
                IdInventario = entity.IdInventario,
                NombreInventario = entity.Inventario?.NombreInventario,
                StockActual = entity.StockActual,
                StockMinimo = entity.StockMinimo,
                Descripcion = entity.Descripcion,
                IdEstadoGeneral = entity.IdEstadoGeneral,
                NombreEstado = entity.EstadoGeneral?.NombreEstado,
                FechaCreacion = entity.FechaCreacion
            };
        }

        protected override Componente MapToEntity(ComponenteCreateDto createDto)
        {
            return new Componente
            {
                NombreComponente = createDto.NombreComponente,
                Marca = createDto.Marca,
                Modelo = createDto.Modelo,
                IdInventario = createDto.IdInventario,
                StockActual = createDto.StockActual,
                StockMinimo = createDto.StockMinimo,
                Descripcion = createDto.Descripcion,
                IdEstadoGeneral = createDto.IdEstadoGeneral,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };
        }

        protected override void MapUpdateToEntity(ComponenteUpdateDto updateDto, Componente entity)
        {
            if (!string.IsNullOrEmpty(updateDto.NombreComponente))
                entity.NombreComponente = updateDto.NombreComponente;
            
            if (updateDto.Marca != null)
                entity.Marca = updateDto.Marca;
            
            if (updateDto.Modelo != null)
                entity.Modelo = updateDto.Modelo;
            
            if (updateDto.IdInventario.HasValue)
                entity.IdInventario = updateDto.IdInventario.Value;
            
            if (updateDto.StockActual.HasValue)
                entity.StockActual = updateDto.StockActual.Value;
            
            if (updateDto.StockMinimo.HasValue)
                entity.StockMinimo = updateDto.StockMinimo.Value;
            
            if (updateDto.Descripcion != null)
                entity.Descripcion = updateDto.Descripcion;
            
            if (updateDto.IdEstadoGeneral.HasValue)
                entity.IdEstadoGeneral = updateDto.IdEstadoGeneral.Value;
        }

        public override async Task<ApiResponseDto<IEnumerable<ComponenteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _componenteRepository.GetAllWithRelationsAsync();
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<ComponenteDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<ComponenteDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<ComponenteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _componenteRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<ComponenteDto>
                    {
                        Success = false,
                        Message = $"Componente con ID {id} no encontrado",
                        Data = null
                    };
                }
                return new ApiResponseDto<ComponenteDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<ComponenteDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<ComponenteDto>> CreateAsync(ComponenteCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _componenteRepository.AddAsync(entity);
                var entityWithRelations = await _componenteRepository.GetByIdWithRelationsAsync(created.Id);
                return new ApiResponseDto<ComponenteDto>
                {
                    Success = true,
                    Message = "Componente creado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<ComponenteDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<ComponenteDto>> UpdateAsync(long id, ComponenteUpdateDto updateDto)
        {
            try
            {
                var entity = await _componenteRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<ComponenteDto>
                    {
                        Success = false,
                        Message = $"Componente con ID {id} no encontrado",
                        Data = null
                    };
                }
                MapUpdateToEntity(updateDto, entity);
                await _componenteRepository.UpdateAsync(entity);
                var entityWithRelations = await _componenteRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<ComponenteDto>
                {
                    Success = true,
                    Message = "Componente actualizado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<ComponenteDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseDto<IEnumerable<ComponenteDto>>> SearchAsync(string? query, int limit = 20)
        {
            try
            {
                var safeLimit = Math.Clamp(limit, 1, 25);
                var entities = await _componenteRepository.SearchWithRelationsAsync(query ?? string.Empty, safeLimit);
                var dtos = entities.Select(MapToDto);

                return new ApiResponseDto<IEnumerable<ComponenteDto>>
                {
                    Success = true,
                    Message = string.IsNullOrWhiteSpace(query) ? "Listado sugerido" : "Busqueda completada",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<ComponenteDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}













