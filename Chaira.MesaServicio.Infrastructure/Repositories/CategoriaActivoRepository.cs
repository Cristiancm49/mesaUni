using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
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













