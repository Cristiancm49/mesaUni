using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IIntervencionTecnicaRepository : IGenericRepository<IntervencionTecnica>
    {
        Task<IEnumerable<IntervencionTecnica>> GetAllWithRelationsAsync();
        Task<IntervencionTecnica?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<IntervencionTecnica>> GetByTrazabilidadIdAsync(long idTrazabilidad);
        Task<IEnumerable<IntervencionTecnica>> GetByCasoIdAsync(long idCaso);
    }
}










