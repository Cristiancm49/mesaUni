using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Application.Services.Inventario
{
    public class HojaDeVidaActivoService : GenericService<HojaDeVidaActivo, HojaDeVidaActivoDto, HojaDeVidaActivoCreateDto, HojaDeVidaActivoCreateDto>, IHojaDeVidaActivoService
    {
        private readonly IHojaDeVidaActivoRepository _hojaRepository;

        public HojaDeVidaActivoService(IHojaDeVidaActivoRepository repository) : base(repository)
        {
            _hojaRepository = repository;
        }

        protected override long GetEntityId(HojaDeVidaActivo entity) => entity.Id;

        protected override HojaDeVidaActivoDto MapToDto(HojaDeVidaActivo entity)
        {
            return new HojaDeVidaActivoDto
            {
                Id = entity.Id,
                IdActivo = entity.IdActivo,
                NombreActivo = entity.Activo?.NombreActivo,
                FechaRegistro = entity.FechaRegistro,
                DetalleRegistro = entity.DetalleRegistro,
                TipoEvento = entity.TipoEvento,
                IdCaso = entity.IdCaso,
                IdUsuarioCreacion = entity.IdUsuarioCreacion,
                NombreUsuario = entity.UsuarioCreacion?.NombreCompleto
            };
        }

        protected override HojaDeVidaActivo MapToEntity(HojaDeVidaActivoCreateDto createDto)
        {
            return new HojaDeVidaActivo
            {
                IdActivo = createDto.IdActivo,
                FechaRegistro = DateTime.Now,
                DetalleRegistro = createDto.DetalleRegistro,
                TipoEvento = createDto.TipoEvento,
                IdCaso = createDto.IdCaso,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion
            };
        }

        public async Task<IEnumerable<HojaDeVidaActivoDto>> GetByCasoIdAsync(long idCaso)
        {
            var hojas = await _hojaRepository.GetByCasoIdAsync(idCaso);
            return hojas.Select(MapToDto);
        }

        protected override void MapUpdateToEntity(HojaDeVidaActivoCreateDto updateDto, HojaDeVidaActivo entity)
        {
            // Las hojas de vida generalmente no se actualizan, solo se agregan nuevas
            // Pero por si acaso:
            if (!string.IsNullOrEmpty(updateDto.DetalleRegistro))
                entity.DetalleRegistro = updateDto.DetalleRegistro;
            
            if (!string.IsNullOrEmpty(updateDto.TipoEvento))
                entity.TipoEvento = updateDto.TipoEvento;
        }

        public override async Task<ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _hojaRepository.GetAllWithRelationsAsync();
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<HojaDeVidaActivoDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _hojaRepository.GetByIdWithRelationsAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<HojaDeVidaActivoDto>
                    {
                        Success = false,
                        Message = $"Registro con ID {id} no encontrado",
                        Data = null
                    };
                }
                return new ApiResponseDto<HojaDeVidaActivoDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<HojaDeVidaActivoDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>> GetByActivoIdAsync(long idActivo)
        {
            try
            {
                var entities = await _hojaRepository.GetByActivoIdAsync(idActivo);
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>
                {
                    Success = true,
                    Message = $"Historial del activo {idActivo}",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResponseDto<HojaDeVidaActivoDto>> CreateAsync(HojaDeVidaActivoCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _hojaRepository.AddAsync(entity);
                var entityWithRelations = await _hojaRepository.GetByIdWithRelationsAsync(created.Id);
                return new ApiResponseDto<HojaDeVidaActivoDto>
                {
                    Success = true,
                    Message = "Registro de hoja de vida creado exitosamente",
                    Data = MapToDto(entityWithRelations!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<HojaDeVidaActivoDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}












