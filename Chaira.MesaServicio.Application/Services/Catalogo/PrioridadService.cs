using Chaira.MesaServicio.Domain.DTOs.Catalogo;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Application.Services.Catalogo
{
    public class PrioridadService : GenericService<Prioridad, PrioridadDto, PrioridadCreateDto, PrioridadUpdateDto>, IPrioridadService
    {
        private readonly IPrioridadRepository _prioridadRepository;

        public PrioridadService(IPrioridadRepository repository) : base(repository)
        {
            _prioridadRepository = repository;
        }

        protected override PrioridadDto MapToDto(Prioridad entity) => new()
        {
            Id = entity.Id,
            NombrePrioridad = entity.NombrePrioridad,
            TiempoRespuestaDias = entity.TiempoRespuestaDias,
            TiempoResolucionDias = entity.TiempoResolucionDias,
            IdEstadoGeneral = entity.IdEstadoGeneral,
            NombreEstado = entity.EstadoGeneral?.NombreEstado,
            FechaCreacion = entity.FechaCreacion
        };

        protected override Prioridad MapToEntity(PrioridadCreateDto dto) => new()
        {
            NombrePrioridad = dto.NombrePrioridad,
            TiempoRespuestaDias = dto.TiempoRespuestaDias,
            TiempoResolucionDias = dto.TiempoResolucionDias,
            IdEstadoGeneral = dto.IdEstadoGeneral,
            FechaCreacion = DateTime.UtcNow,
            IdUsuarioCreacion = dto.IdUsuarioCreacion
        };

        protected override void MapUpdateToEntity(PrioridadUpdateDto dto, Prioridad entity)
        {
            if (!string.IsNullOrEmpty(dto.NombrePrioridad)) entity.NombrePrioridad = dto.NombrePrioridad;
            if (dto.TiempoRespuestaDias.HasValue) entity.TiempoRespuestaDias = dto.TiempoRespuestaDias.Value;
            if (dto.TiempoResolucionDias.HasValue) entity.TiempoResolucionDias = dto.TiempoResolucionDias.Value;
            if (dto.IdEstadoGeneral.HasValue) entity.IdEstadoGeneral = dto.IdEstadoGeneral.Value;
        }

        protected override long GetEntityId(Prioridad entity) => entity.Id;

        public override async Task<ApiResponseDto<IEnumerable<PrioridadDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _prioridadRepository.GetAllWithRelationsAsync();
                return new ApiResponseDto<IEnumerable<PrioridadDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = entities.Select(MapToDto)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<PrioridadDto>>
                {
                    Success = false,
                    Message = "Error: " + ex.Message,
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<PrioridadDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _prioridadRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                    return new ApiResponseDto<PrioridadDto> { Success = false, Message = "Registro no encontrado", Data = null };

                return new ApiResponseDto<PrioridadDto> { Success = true, Message = "Registro obtenido", Data = MapToDto(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<PrioridadDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<PrioridadDto>> CreateAsync(PrioridadCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _repository.AddAsync(entity);
                var entityWithRelations = await _prioridadRepository.GetByIdWithRelationsAsync(GetEntityId(created));
                return new ApiResponseDto<PrioridadDto> { Success = true, Message = "Registro creado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<PrioridadDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<PrioridadDto>> UpdateAsync(long id, PrioridadUpdateDto updateDto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponseDto<PrioridadDto> { Success = false, Message = "Registro no encontrado", Data = null };

                MapUpdateToEntity(updateDto, entity);
                await _repository.UpdateAsync(entity);
                var entityWithRelations = await _prioridadRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<PrioridadDto> { Success = true, Message = "Registro actualizado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<PrioridadDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }
    }
}













