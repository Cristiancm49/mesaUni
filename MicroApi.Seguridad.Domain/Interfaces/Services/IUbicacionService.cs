using MicroApi.Seguridad.Domain.DTOs.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IUbicacionService
    {
        Task<IEnumerable<UbicacionDto>> GetAllAsync();
        Task<UbicacionDto?> GetByIdAsync(long id);
        Task<UbicacionDto> CreateAsync(UbicacionCreateDto dto);
        Task<UbicacionDto?> UpdateAsync(long id, UbicacionUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
