using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ICategoriaActivoRepository : IGenericRepository<CategoriaActivo>
    {
        Task<IEnumerable<CategoriaActivo>> GetAllWithRelationsAsync();
        Task<CategoriaActivo?> GetByIdWithRelationsAsync(long id);
    }
}












