using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Inventario;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IHojaDeVidaActivoService : IGenericService<HojaDeVidaActivo, HojaDeVidaActivoDto, HojaDeVidaActivoCreateDto, HojaDeVidaActivoCreateDto>
    {
        Task<ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>> GetByActivoIdAsync(long idActivo);
        Task<IEnumerable<HojaDeVidaActivoDto>> GetByCasoIdAsync(long idCaso);
    }
}











