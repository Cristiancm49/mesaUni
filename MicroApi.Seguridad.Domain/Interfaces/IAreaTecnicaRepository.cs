using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IAreaTecnicaRepository : IGenericRepository<AreaTecnica>
    {
        Task<IEnumerable<AreaTecnica>> GetAllWithRelationsAsync();
        Task<AreaTecnica?> GetByIdWithRelationsAsync(long id);
    }
}
