using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IDetalleCambioComponentesRepository : IGenericRepository<DetalleCambioComponentes>
    {
        Task<IEnumerable<DetalleCambioComponentes>> GetAllWithRelationsAsync();
        Task<DetalleCambioComponentes?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<DetalleCambioComponentes>> GetByIntervencionIdAsync(long idIntervencion);
    }
}











