using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class CategoriaActivoRepository : GenericRepository<CategoriaActivo>, ICategoriaActivoRepository
    {
        public CategoriaActivoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CategoriaActivo>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(c => c.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<CategoriaActivo?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(c => c.EstadoGeneral)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}












