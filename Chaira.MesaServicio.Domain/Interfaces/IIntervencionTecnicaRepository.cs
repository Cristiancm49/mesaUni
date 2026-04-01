using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IIntervencionTecnicaRepository : IGenericRepository<IntervencionTecnica>
    {
        Task<IEnumerable<IntervencionTecnica>> GetAllWithRelationsAsync();
        Task<IntervencionTecnica?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<IntervencionTecnica>> GetByTrazabilidadIdAsync(long idTrazabilidad);
        Task<IEnumerable<IntervencionTecnica>> GetByCasoIdAsync(long idCaso);
    }
}











