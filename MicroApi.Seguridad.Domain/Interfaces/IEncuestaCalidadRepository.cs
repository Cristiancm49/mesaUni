using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IEncuestaCalidadRepository : IGenericRepository<EncuestaCalidad>
    {
        Task<IEnumerable<EncuestaCalidad>> GetAllWithRelationsAsync();
        Task<EncuestaCalidad?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<EncuestaCalidad>> GetByCasoIdAsync(long idCaso);
    }
}











