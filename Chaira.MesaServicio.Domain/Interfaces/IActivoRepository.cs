using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IActivoRepository : IGenericRepository<Activo>
    {
        Task<IEnumerable<Activo>> GetAllWithRelationsAsync();
        Task<Activo?> GetByIdWithRelationsAsync(long id);
    }
}













