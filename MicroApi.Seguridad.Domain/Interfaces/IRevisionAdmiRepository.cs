using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IRevisionAdmiRepository : IGenericRepository<RevisionAdmi>
    {
        Task<IEnumerable<RevisionAdmi>> GetAllWithRelationsAsync();
        Task<RevisionAdmi?> GetByIdWithRelationsAsync(long id);
        Task<RevisionAdmi?> GetByIntervencionIdAsync(long idIntervencion);
    }
}











