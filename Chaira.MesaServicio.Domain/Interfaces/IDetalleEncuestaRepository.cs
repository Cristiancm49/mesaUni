using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IDetalleEncuestaRepository : IGenericRepository<DetalleEncuesta>
    {
        Task<IEnumerable<DetalleEncuesta>> GetAllWithRelationsAsync();
        Task<DetalleEncuesta?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<DetalleEncuesta>> GetByEncuestaIdAsync(long idEncuesta);
    }
}












