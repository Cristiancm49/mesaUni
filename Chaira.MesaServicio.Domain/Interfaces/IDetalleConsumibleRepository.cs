using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IDetalleConsumibleRepository : IGenericRepository<DetalleConsumible>
    {
        Task<IEnumerable<DetalleConsumible>> GetAllWithRelationsAsync();
        Task<DetalleConsumible?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<DetalleConsumible>> GetByIntervencionIdAsync(long idIntervencion);
    }
}












