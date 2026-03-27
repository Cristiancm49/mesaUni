using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IUbicacionRepository : IGenericRepository<Ubicacion>
    {
        Task<IEnumerable<Ubicacion>> GetAllWithSedeAsync();
        Task<Ubicacion?> GetByIdWithSedeAsync(long id);
        Task<bool> DeleteAsync(long id);
    }
}

