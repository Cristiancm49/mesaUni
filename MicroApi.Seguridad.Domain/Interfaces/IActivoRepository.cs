using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IActivoRepository : IGenericRepository<Activo>
    {
        Task<IEnumerable<Activo>> GetAllWithRelationsAsync();
        Task<Activo?> GetByIdWithRelationsAsync(long id);
    }
}












