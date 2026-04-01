using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface ICategoriaActivoRepository : IGenericRepository<CategoriaActivo>
    {
        Task<IEnumerable<CategoriaActivo>> GetAllWithRelationsAsync();
        Task<CategoriaActivo?> GetByIdWithRelationsAsync(long id);
    }
}













