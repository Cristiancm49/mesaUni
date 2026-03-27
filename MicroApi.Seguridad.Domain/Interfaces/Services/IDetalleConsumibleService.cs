using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IDetalleConsumibleService
    {
        Task<IEnumerable<DetalleConsumibleDto>> GetAllAsync();
        Task<DetalleConsumibleDto?> GetByIdAsync(long id);
        Task<IEnumerable<DetalleConsumibleDto>> GetByIntervencionIdAsync(long idIntervencion);
        Task<DetalleConsumibleDto> CreateAsync(DetalleConsumibleCreateDto dto);
        Task<int> CountAsync();
    }
}











