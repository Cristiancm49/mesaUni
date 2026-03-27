using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ITipoCasoRepository : IGenericRepository<TipoCaso>
    {
        Task<IEnumerable<TipoCaso>> GetAllWithRelationsAsync();
        Task<TipoCaso?> GetByIdWithRelationsAsync(long id);
    }
}
