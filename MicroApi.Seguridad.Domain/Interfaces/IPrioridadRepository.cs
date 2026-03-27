using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IPrioridadRepository : IGenericRepository<Prioridad>
    {
        Task<IEnumerable<Prioridad>> GetAllWithRelationsAsync();
        Task<Prioridad?> GetByIdWithRelationsAsync(long id);
    }
}
