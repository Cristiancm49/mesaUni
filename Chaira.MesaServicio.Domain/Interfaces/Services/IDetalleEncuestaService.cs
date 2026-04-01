using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IDetalleEncuestaService
    {
        Task<IEnumerable<DetalleEncuestaDto>> GetAllAsync();
        Task<DetalleEncuestaDto?> GetByIdAsync(long id);
        Task<IEnumerable<DetalleEncuestaDto>> GetByEncuestaIdAsync(long idEncuesta);
        Task<DetalleEncuestaDto> CreateAsync(DetalleEncuestaCreateDto dto);
        Task<int> CountAsync();
    }
}












