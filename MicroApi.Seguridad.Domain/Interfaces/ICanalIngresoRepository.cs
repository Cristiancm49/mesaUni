using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ICanalIngresoRepository : IGenericRepository<CanalIngreso>
    {
        Task<IEnumerable<CanalIngreso>> GetAllWithRelationsAsync();
        Task<CanalIngreso?> GetByIdWithRelationsAsync(long id);
    }
}
