using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IAreaTecnicaRepository : IGenericRepository<AreaTecnica>
    {
        Task<IEnumerable<AreaTecnica>> GetAllWithRelationsAsync();
        Task<AreaTecnica?> GetByIdWithRelationsAsync(long id);
    }
}

