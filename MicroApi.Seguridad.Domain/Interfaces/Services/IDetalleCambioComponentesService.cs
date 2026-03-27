using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IDetalleCambioComponentesService
    {
        Task<IEnumerable<DetalleCambioComponentesDto>> GetAllAsync();
        Task<DetalleCambioComponentesDto?> GetByIdAsync(long id);
        Task<IEnumerable<DetalleCambioComponentesDto>> GetByIntervencionIdAsync(long idIntervencion);
        Task<DetalleCambioComponentesDto> CreateAsync(DetalleCambioComponentesCreateDto dto);
        Task<int> CountAsync();
    }
}











