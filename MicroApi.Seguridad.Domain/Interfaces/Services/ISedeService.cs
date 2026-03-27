using MicroApi.Seguridad.Domain.DTOs.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface ISedeService
    {
        Task<IEnumerable<SedeDto>> GetAllAsync();
        Task<SedeDto?> GetByIdAsync(long id);
        Task<SedeDto> CreateAsync(SedeCreateDto dto);
        Task<SedeDto?> UpdateAsync(long id, SedeUpdateDto dto);
    }
}










