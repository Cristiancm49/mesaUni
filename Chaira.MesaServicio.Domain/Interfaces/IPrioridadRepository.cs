using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IPrioridadRepository : IGenericRepository<Prioridad>
    {
        Task<IEnumerable<Prioridad>> GetAllWithRelationsAsync();
        Task<Prioridad?> GetByIdWithRelationsAsync(long id);
    }
}

