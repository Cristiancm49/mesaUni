using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IHojaDeVidaActivoService : IGenericService<HojaDeVidaActivo, HojaDeVidaActivoDto, HojaDeVidaActivoCreateDto, HojaDeVidaActivoCreateDto>
    {
        Task<ApiResponseDto<IEnumerable<HojaDeVidaActivoDto>>> GetByActivoIdAsync(long idActivo);
        Task<IEnumerable<HojaDeVidaActivoDto>> GetByCasoIdAsync(long idCaso);
    }
}












