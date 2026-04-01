using Chaira.MesaServicio.Domain.DTOs.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface ISedeService
    {
        Task<IEnumerable<SedeDto>> GetAllAsync();
        Task<SedeDto?> GetByIdAsync(long id);
        Task<SedeDto> CreateAsync(SedeCreateDto dto);
        Task<SedeDto?> UpdateAsync(long id, SedeUpdateDto dto);
    }
}











