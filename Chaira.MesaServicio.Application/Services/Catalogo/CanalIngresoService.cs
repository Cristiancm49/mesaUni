using Chaira.MesaServicio.Domain.DTOs.Catalogo;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Application.Services.Catalogo
{
    public class CanalIngresoService : GenericService<CanalIngreso, CanalIngresoDto, CanalIngresoCreateDto, CanalIngresoUpdateDto>, ICanalIngresoService
    {
        private readonly ICanalIngresoRepository _canalIngresoRepository;

        public CanalIngresoService(ICanalIngresoRepository repository) : base(repository)
        {
            _canalIngresoRepository = repository;
        }

        protected override CanalIngresoDto MapToDto(CanalIngreso entity) => new()
        {
            Id = entity.Id,
            NombreCanal = entity.NombreCanal,
            Descripcion = entity.Descripcion,
            IdEstadoGeneral = entity.IdEstadoGeneral,
            NombreEstado = entity.EstadoGeneral?.NombreEstado,
            FechaCreacion = entity.FechaCreacion
        };

        protected override CanalIngreso MapToEntity(CanalIngresoCreateDto dto) => new()
        {
            NombreCanal = dto.NombreCanal,
            Descripcion = dto.Descripcion,
            IdEstadoGeneral = dto.IdEstadoGeneral,
            FechaCreacion = DateTime.UtcNow,
            IdUsuarioCreacion = dto.IdUsuarioCreacion
        };

        protected override void MapUpdateToEntity(CanalIngresoUpdateDto dto, CanalIngreso entity)
        {
            if (!string.IsNullOrEmpty(dto.NombreCanal)) entity.NombreCanal = dto.NombreCanal;
            if (dto.Descripcion != null) entity.Descripcion = dto.Descripcion;
            if (dto.IdEstadoGeneral.HasValue) entity.IdEstadoGeneral = dto.IdEstadoGeneral.Value;
        }

        protected override long GetEntityId(CanalIngreso entity) => entity.Id;

        public override async Task<ApiResponseDto<IEnumerable<CanalIngresoDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _canalIngresoRepository.GetAllWithRelationsAsync();
                return new ApiResponseDto<IEnumerable<CanalIngresoDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = entities.Select(MapToDto)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<CanalIngresoDto>>
                {
                    Success = false,
                    Message = "Error: " + ex.Message,
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<CanalIngresoDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _canalIngresoRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                    return new ApiResponseDto<CanalIngresoDto> { Success = false, Message = "Registro no encontrado", Data = null };

                return new ApiResponseDto<CanalIngresoDto> { Success = true, Message = "Registro obtenido", Data = MapToDto(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<CanalIngresoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<CanalIngresoDto>> CreateAsync(CanalIngresoCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _repository.AddAsync(entity);
                var entityWithRelations = await _canalIngresoRepository.GetByIdWithRelationsAsync(GetEntityId(created));
                return new ApiResponseDto<CanalIngresoDto> { Success = true, Message = "Registro creado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<CanalIngresoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<CanalIngresoDto>> UpdateAsync(long id, CanalIngresoUpdateDto updateDto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponseDto<CanalIngresoDto> { Success = false, Message = "Registro no encontrado", Data = null };

                MapUpdateToEntity(updateDto, entity);
                await _repository.UpdateAsync(entity);
                var entityWithRelations = await _canalIngresoRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<CanalIngresoDto> { Success = true, Message = "Registro actualizado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<CanalIngresoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }
    }
}













