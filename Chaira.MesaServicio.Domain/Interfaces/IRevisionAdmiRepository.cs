using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IRevisionAdmiRepository : IGenericRepository<RevisionAdmi>
    {
        Task<IEnumerable<RevisionAdmi>> GetAllWithRelationsAsync();
        Task<RevisionAdmi?> GetByIdWithRelationsAsync(long id);
        Task<RevisionAdmi?> GetByIntervencionIdAsync(long idIntervencion);
    }
}












