using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Data.Repositories
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












