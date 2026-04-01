using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IUbicacionRepository : IGenericRepository<Ubicacion>
    {
        Task<IEnumerable<Ubicacion>> GetAllWithSedeAsync();
        Task<Ubicacion?> GetByIdWithSedeAsync(long id);
        Task<bool> DeleteAsync(long id);
    }
}


