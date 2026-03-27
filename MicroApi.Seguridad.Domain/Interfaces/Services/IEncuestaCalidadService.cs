using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IEncuestaCalidadService
    {
        Task<IEnumerable<EncuestaCalidadDto>> GetAllAsync();
        Task<EncuestaCalidadDto?> GetByIdAsync(long id);
        Task<IEnumerable<EncuestaCalidadDto>> GetByCasoIdAsync(long idCaso);
        Task<EncuestaCalidadDto> CreateAsync(EncuestaCalidadCreateDto dto);
        Task<int> CountAsync();
    }
}











