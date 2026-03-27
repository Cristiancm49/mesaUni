using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ITrazabilidadCasoRepository : IGenericRepository<TrazabilidadCaso>
    {
        Task<IEnumerable<TrazabilidadCaso>> GetAllWithRelationsAsync();
        Task<TrazabilidadCaso?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<TrazabilidadCaso>> GetByCasoIdAsync(long idCaso);
    }
}











