using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ITipoConsumibleRepository : IGenericRepository<TipoConsumible>
    {
        Task<IEnumerable<TipoConsumible>> GetAllWithRelationsAsync();
        Task<TipoConsumible?> GetByIdWithRelationsAsync(long id);
    }
}












