using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class InventarioRepository : GenericRepository<Inventario>, IInventarioRepository
    {
        public InventarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Inventario>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(i => i.EstadoGeneral)
                .Include(i => i.Responsable)
                .ToListAsync();
        }

        public async Task<Inventario?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(i => i.EstadoGeneral)
                .Include(i => i.Responsable)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}













