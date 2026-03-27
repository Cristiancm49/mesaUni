using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Inventario;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Application.Services.Inventario
{
    public class ConsumibleService : GenericService<Consumible, ConsumibleDto, ConsumibleCreateDto, ConsumibleUpdateDto>, IConsumibleService
    {
        private readonly IConsumibleRepository _consumibleRepository;

        public ConsumibleService(IConsumibleRepository repository) : base(repository)
        {
            _consumibleRepository = repository;
        }

        protected override long GetEntityId(Consumible entity) => entity.Id;

        protected override ConsumibleDto MapToDto(Consumible entity)
        {
            return new ConsumibleDto
            {
                Id = entity.Id,
                NombreConsumible = entity.NombreConsumible,
                Marca = entity.Marca,
                Modelo = entity.Modelo,
                StockActual = entity.StockActual,
                StockMinimo = entity.StockMinimo,
                DescripcionTecnica = entity.DescripcionTecnica,
                IdInventario = entity.IdInventario,
                NombreInventario = entity.Inventario?.NombreInventario,
                IdTipoConsumible = entity.IdTipoConsumible,
                NombreTipo = entity.TipoConsumible?.NombreTipo,
                IdEstadoConsumible = entity.IdEstadoConsumible,
                NombreEstado = entity.EstadoConsumible?.NombreEstado,
                FechaCreacion = entity.FechaCreacion,
                FechaActualizacion = entity.FechaActualizacion
            };
        }

        protected override Consumible MapToEntity(ConsumibleCreateDto createDto)
        {
            return new Consumible
            {
                NombreConsumible = createDto.NombreConsumible,
                Marca = createDto.Marca,
                Modelo = createDto.Modelo,
                StockActual = createDto.StockActual,
                StockMinimo = createDto.StockMinimo,
                DescripcionTecnica = createDto.DescripcionTecnica,
                IdInventario = createDto.IdInventario,
                IdTipoConsumible = createDto.IdTipoConsumible,
                IdEstadoConsumible = createDto.IdEstadoConsumible,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };
        }

        protected override void MapUpdateToEntity(ConsumibleUpdateDto updateDto, Consumible entity)
        {
            if (!string.IsNullOrEmpty(updateDto.NombreConsumible))
                entity.NombreConsumible = updateDto.NombreConsumible;
            
            if (updateDto.Marca != null)
                entity.Marca = updateDto.Marca;
            
            if (updateDto.Modelo != null)
                entity.Modelo = updateDto.Modelo;
            
            if (updateDto.StockActual.HasValue)
                entity.StockActual = updateDto.StockActual.Value;
            
            if (updateDto.StockMinimo.HasValue)
                entity.StockMinimo = updateDto.StockMinimo.Value;
            
            if (updateDto.DescripcionTecnica != null)
                entity.DescripcionTecnica = updateDto.DescripcionTecnica;
            
            if (updateDto.IdInventario.HasValue)
                entity.IdInventario = updateDto.IdInventario.Value;
            
            if (updateDto.IdTipoConsumible.HasValue)
                entity.IdTipoConsumible = updateDto.IdTipoConsumible.Value;
            
            if (updateDto.IdEstadoConsumible.HasValue)
                entity.IdEstadoConsumible = updateDto.IdEstadoConsumible.Value;
            
            entity.FechaActualizacion = DateTime.Now;
        }

        public override async Task<ApiResponseDto<IEnumerable<ConsumibleDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _consumibleRepository.GetAllWithRelationsAsync();
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<ConsumibleDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<ConsumibleDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<ConsumibleDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _consumibleRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<ConsumibleDto>
                    {
                        Success = false,
                        Message = $"Consumible con ID {id} no encontrado",
                        Data = null
                    };
                }
                return new ApiResponseDto<ConsumibleDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<ConsumibleDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<ConsumibleDto>> CreateAsync(ConsumibleCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _consumibleRepository.AddAsync(entity);
                var entityWithRelations = await _consumibleRepository.GetByIdWithRelationsAsync(created.Id);
                return new ApiResponseDto<ConsumibleDto>
                {
                    Success = true,
                    Message = "Consumible creado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<ConsumibleDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<ConsumibleDto>> UpdateAsync(long id, ConsumibleUpdateDto updateDto)
        {
            try
            {
                var entity = await _consumibleRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<ConsumibleDto>
                    {
                        Success = false,
                        Message = $"Consumible con ID {id} no encontrado",
                        Data = null
                    };
                }
                MapUpdateToEntity(updateDto, entity);
                await _consumibleRepository.UpdateAsync(entity);
                var entityWithRelations = await _consumibleRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<ConsumibleDto>
                {
                    Success = true,
                    Message = "Consumible actualizado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<ConsumibleDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseDto<IEnumerable<ConsumibleDto>>> SearchAsync(string? query, int limit = 20)
        {
            try
            {
                var safeLimit = Math.Clamp(limit, 1, 25);
                var entities = await _consumibleRepository.SearchWithRelationsAsync(query ?? string.Empty, safeLimit);
                var dtos = entities.Select(MapToDto);

                return new ApiResponseDto<IEnumerable<ConsumibleDto>>
                {
                    Success = true,
                    Message = string.IsNullOrWhiteSpace(query) ? "Listado sugerido" : "Busqueda completada",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<ConsumibleDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}












