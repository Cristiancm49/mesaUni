using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IInventarioRepository : IGenericRepository<Inventario>
    {
        Task<IEnumerable<Inventario>> GetAllWithRelationsAsync();
        Task<Inventario?> GetByIdWithRelationsAsync(long id);
    }
}
