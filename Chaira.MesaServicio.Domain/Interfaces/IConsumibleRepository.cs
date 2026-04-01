using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IConsumibleRepository : IGenericRepository<Consumible>
    {
        Task<IEnumerable<Consumible>> GetAllWithRelationsAsync();
        Task<Consumible?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<Consumible>> SearchWithRelationsAsync(string query, int limit);
    }
}













