using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IRevisionAdmiService
    {
        Task<IEnumerable<RevisionAdmiDto>> GetAllAsync();
        Task<RevisionAdmiDto?> GetByIdAsync(long id);
        Task<RevisionAdmiDto?> GetByIntervencionIdAsync(long idIntervencion);
        Task<RevisionAdmiDto> CreateAsync(RevisionAdmiCreateDto dto);
        Task<RevisionAdmiDto?> UpdateAsync(RevisionAdmiUpdateDto dto);
        Task<int> CountAsync();
    }
}












