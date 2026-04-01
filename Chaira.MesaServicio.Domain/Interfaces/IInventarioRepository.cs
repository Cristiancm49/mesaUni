using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IInventarioRepository : IGenericRepository<Inventario>
    {
        Task<IEnumerable<Inventario>> GetAllWithRelationsAsync();
        Task<Inventario?> GetByIdWithRelationsAsync(long id);
    }
}

