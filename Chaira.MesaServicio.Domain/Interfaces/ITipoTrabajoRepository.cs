using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface ITipoTrabajoRepository : IGenericRepository<TipoTrabajo>
    {
        Task<IEnumerable<TipoTrabajo>> GetAllWithRelationsAsync();
        Task<TipoTrabajo?> GetByIdWithRelationsAsync(long id);
    }
}

