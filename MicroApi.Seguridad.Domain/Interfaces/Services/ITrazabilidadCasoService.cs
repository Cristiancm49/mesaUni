using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface ITrazabilidadCasoService
    {
        Task<IEnumerable<TrazabilidadCasoDto>> GetAllAsync();
        Task<TrazabilidadCasoDto?> GetByIdAsync(long id);
        Task<IEnumerable<TrazabilidadCasoDto>> GetByCasoIdAsync(long idCaso);
        Task<TrazabilidadCasoDto> CreateAsync(TrazabilidadCasoCreateDto dto);
        Task<int> CountAsync();
    }
}











