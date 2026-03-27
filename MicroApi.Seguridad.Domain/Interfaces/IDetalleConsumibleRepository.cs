using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IDetalleConsumibleRepository : IGenericRepository<DetalleConsumible>
    {
        Task<IEnumerable<DetalleConsumible>> GetAllWithRelationsAsync();
        Task<DetalleConsumible?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<DetalleConsumible>> GetByIntervencionIdAsync(long idIntervencion);
    }
}











