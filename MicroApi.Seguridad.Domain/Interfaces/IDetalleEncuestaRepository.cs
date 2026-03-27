using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IDetalleEncuestaRepository : IGenericRepository<DetalleEncuesta>
    {
        Task<IEnumerable<DetalleEncuesta>> GetAllWithRelationsAsync();
        Task<DetalleEncuesta?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<DetalleEncuesta>> GetByEncuestaIdAsync(long idEncuesta);
    }
}











