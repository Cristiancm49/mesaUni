using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IIntervencionTecnicaService
    {
        Task<IEnumerable<IntervencionTecnicaDto>> GetAllAsync();
        Task<IntervencionTecnicaDto?> GetByIdAsync(long id);
        Task<IEnumerable<IntervencionTecnicaDto>> GetByTrazabilidadIdAsync(long idTrazabilidad);
        Task<IEnumerable<IntervencionTecnicaDto>> GetByCasoIdAsync(long idCaso);
        Task<IntervencionTecnicaDto> CreateAsync(IntervencionTecnicaCreateDto dto);
        Task<IntervencionTecnicaDto?> UpdateAsync(IntervencionTecnicaUpdateDto dto);
        Task<int> CountAsync();
    }
}











