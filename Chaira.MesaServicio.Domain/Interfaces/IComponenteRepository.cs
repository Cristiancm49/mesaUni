using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IComponenteRepository : IGenericRepository<Componente>
    {
        Task<IEnumerable<Componente>> GetAllWithRelationsAsync();
        Task<Componente?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<Componente>> SearchWithRelationsAsync(string query, int limit);
    }
}













