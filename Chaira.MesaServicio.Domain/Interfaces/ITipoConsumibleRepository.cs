using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface ITipoConsumibleRepository : IGenericRepository<TipoConsumible>
    {
        Task<IEnumerable<TipoConsumible>> GetAllWithRelationsAsync();
        Task<TipoConsumible?> GetByIdWithRelationsAsync(long id);
    }
}













