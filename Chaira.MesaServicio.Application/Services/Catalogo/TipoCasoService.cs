using Chaira.MesaServicio.Domain.DTOs.Catalogo;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Application.Services.Catalogo
{
    public class TipoCasoService : GenericService<TipoCaso, TipoCasoDto, TipoCasoCreateDto, TipoCasoUpdateDto>, ITipoCasoService
    {
        private readonly ITipoCasoRepository _tipoCasoRepository;

        public TipoCasoService(ITipoCasoRepository repository) : base(repository)
        {
            _tipoCasoRepository = repository;
        }

        protected override TipoCasoDto MapToDto(TipoCaso entity) => new()
        {
            Id = entity.Id,
            NombreTipoCaso = entity.NombreTipoCaso,
            Descripcion = entity.Descripcion,
            IdEstadoGeneral = entity.IdEstadoGeneral,
            NombreEstado = entity.EstadoGeneral?.NombreEstado,
            FechaCreacion = entity.FechaCreacion
        };

        protected override TipoCaso MapToEntity(TipoCasoCreateDto dto) => new()
        {
            NombreTipoCaso = dto.NombreTipoCaso,
            Descripcion = dto.Descripcion,
            IdEstadoGeneral = dto.IdEstadoGeneral,
            FechaCreacion = DateTime.UtcNow,
            IdUsuarioCreacion = dto.IdUsuarioCreacion
        };

        protected override void MapUpdateToEntity(TipoCasoUpdateDto dto, TipoCaso entity)
        {
            if (!string.IsNullOrEmpty(dto.NombreTipoCaso)) entity.NombreTipoCaso = dto.NombreTipoCaso;
            if (dto.Descripcion != null) entity.Descripcion = dto.Descripcion;
            if (dto.IdEstadoGeneral.HasValue) entity.IdEstadoGeneral = dto.IdEstadoGeneral.Value;
        }

        protected override long GetEntityId(TipoCaso entity) => entity.Id;

        public override async Task<ApiResponseDto<IEnumerable<TipoCasoDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _tipoCasoRepository.GetAllWithRelationsAsync();
                return new ApiResponseDto<IEnumerable<TipoCasoDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = entities.Select(MapToDto)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<TipoCasoDto>>
                {
                    Success = false,
                    Message = "Error: " + ex.Message,
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<TipoCasoDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _tipoCasoRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                    return new ApiResponseDto<TipoCasoDto> { Success = false, Message = "Registro no encontrado", Data = null };

                return new ApiResponseDto<TipoCasoDto> { Success = true, Message = "Registro obtenido", Data = MapToDto(entity) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoCasoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<TipoCasoDto>> CreateAsync(TipoCasoCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _repository.AddAsync(entity);
                var entityWithRelations = await _tipoCasoRepository.GetByIdWithRelationsAsync(GetEntityId(created));
                return new ApiResponseDto<TipoCasoDto> { Success = true, Message = "Registro creado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoCasoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }

        public override async Task<ApiResponseDto<TipoCasoDto>> UpdateAsync(long id, TipoCasoUpdateDto updateDto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return new ApiResponseDto<TipoCasoDto> { Success = false, Message = "Registro no encontrado", Data = null };

                MapUpdateToEntity(updateDto, entity);
                await _repository.UpdateAsync(entity);
                var entityWithRelations = await _tipoCasoRepository.GetByIdWithRelationsAsync(id);
                return new ApiResponseDto<TipoCasoDto> { Success = true, Message = "Registro actualizado", Data = MapToDto(entityWithRelations!) };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<TipoCasoDto> { Success = false, Message = "Error: " + ex.Message, Data = null };
            }
        }
    }
}













