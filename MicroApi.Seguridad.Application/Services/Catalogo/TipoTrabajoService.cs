using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Application.Services.Catalogo
{
    public class TipoTrabajoService : GenericService<TipoTrabajo, TipoTrabajoDto, TipoTrabajoCreateDto, TipoTrabajoUpdateDto>, ITipoTrabajoService
    {
        private readonly ITipoTrabajoRepository _tipoTrabajoRepository;

        public TipoTrabajoService(ITipoTrabajoRepository repository) : base(repository)
        {
            _tipoTrabajoRepository = repository;
        }

        protected override TipoTrabajoDto MapToDto(TipoTrabajo entity) => new()
        {
            Id = entity.Id,
            NombreTipoTrabajo = entity.NombreTipoTrabajo,
            Descripcion = entity.Descripcion,
            IdEstadoGeneral = entity.IdEstadoGeneral,
            NombreEstado = entity.EstadoGeneral?.NombreEstado,
            FechaCreacion = entity.FechaCreacion
        };

        protected override TipoTrabajo MapToEntity(TipoTrabajoCreateDto dto) => new()
        {
            NombreTipoTrabajo = dto.NombreTipoTrabajo,
            Descripcion = dto.Descripcion,
            IdEstadoGeneral = dto.IdEstadoGeneral,
            FechaCreacion = DateTime.UtcNow,
            IdUsuarioCreacion = dto.IdUsuarioCreacion
        };

        protected override void MapUpdateToEntity(TipoTrabajoUpdateDto dto, TipoTrabajo entity)
        {
            if (!string.IsNullOrEmpty(dto.NombreTipoTrabajo)) entity.NombreTipoTrabajo = dto.NombreTipoTrabajo;
            if (dto.Descripcion != null) entity.Descripcion = dto.Descripcion;
            if (dto.IdEstadoGeneral.HasValue) entity.IdEstadoGeneral = dto.IdEstadoGeneral.Value;
        }

        protected override long GetEntityId(TipoTrabajo entity) => entity.Id;

        public override async Task<ApiResponseDto<IEnumerable<TipoTrabajoDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _tipoTrabajoRepository.GetAllWithRelationsAsync();
                return new ApiResponseDto<IEnumerable<TipoTrabajoDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = entities.Select(MapToDto)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<TipoTrabajoDto>>
                {
                    Success = false,
                    Message = "Error: " + ex.Message,
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<TipoTrabajoDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _tipoTrabajoRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                    return new ApiResponseDto<TipoTrabajoDto> { Success = false, Message = "Registro no encontrado", Data = null };

                return new ApiResponseDto<TipoTrabajoDto> { Success = true, Message = "Registro obtenido", Data = MapToDto(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoTrabajoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<TipoTrabajoDto>> CreateAsync(TipoTrabajoCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _repository.AddAsync(entity);
                var entityWithRelations = await _tipoTrabajoRepository.GetByIdWithRelationsAsync(GetEntityId(created));
                return new ApiResponseDto<TipoTrabajoDto> { Success = true, Message = "Registro creado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoTrabajoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<TipoTrabajoDto>> UpdateAsync(long id, TipoTrabajoUpdateDto updateDto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponseDto<TipoTrabajoDto> { Success = false, Message = "Registro no encontrado", Data = null };

                MapUpdateToEntity(updateDto, entity);
                await _repository.UpdateAsync(entity);
                var entityWithRelations = await _tipoTrabajoRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<TipoTrabajoDto> { Success = true, Message = "Registro actualizado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoTrabajoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }
    }
}












