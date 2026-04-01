using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface ITipoCasoRepository : IGenericRepository<TipoCaso>
    {
        Task<IEnumerable<TipoCaso>> GetAllWithRelationsAsync();
        Task<TipoCaso?> GetByIdWithRelationsAsync(long id);
    }
}

