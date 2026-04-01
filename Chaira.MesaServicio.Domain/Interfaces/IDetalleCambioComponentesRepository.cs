using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IDetalleCambioComponentesRepository : IGenericRepository<DetalleCambioComponentes>
    {
        Task<IEnumerable<DetalleCambioComponentes>> GetAllWithRelationsAsync();
        Task<DetalleCambioComponentes?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<DetalleCambioComponentes>> GetByIntervencionIdAsync(long idIntervencion);
    }
}












