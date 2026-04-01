using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface ICanalIngresoRepository : IGenericRepository<CanalIngreso>
    {
        Task<IEnumerable<CanalIngreso>> GetAllWithRelationsAsync();
        Task<CanalIngreso?> GetByIdWithRelationsAsync(long id);
    }
}

