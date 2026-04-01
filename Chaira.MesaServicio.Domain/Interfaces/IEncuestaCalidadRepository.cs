using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IEncuestaCalidadRepository : IGenericRepository<EncuestaCalidad>
    {
        Task<IEnumerable<EncuestaCalidad>> GetAllWithRelationsAsync();
        Task<EncuestaCalidad?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<EncuestaCalidad>> GetByCasoIdAsync(long idCaso);
    }
}












