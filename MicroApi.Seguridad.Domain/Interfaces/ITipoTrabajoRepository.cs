using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ITipoTrabajoRepository : IGenericRepository<TipoTrabajo>
    {
        Task<IEnumerable<TipoTrabajo>> GetAllWithRelationsAsync();
        Task<TipoTrabajo?> GetByIdWithRelationsAsync(long id);
    }
}
