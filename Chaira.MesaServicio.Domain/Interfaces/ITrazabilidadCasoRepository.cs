using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface ITrazabilidadCasoRepository : IGenericRepository<TrazabilidadCaso>
    {
        Task<IEnumerable<TrazabilidadCaso>> GetAllWithRelationsAsync();
        Task<TrazabilidadCaso?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<TrazabilidadCaso>> GetByCasoIdAsync(long idCaso);
    }
}












