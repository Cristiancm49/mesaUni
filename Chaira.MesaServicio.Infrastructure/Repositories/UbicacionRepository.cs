using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class UbicacionRepository : GenericRepository<Ubicacion>, IUbicacionRepository
    {
        public UbicacionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ubicacion>> GetAllWithSedeAsync()
        {
            return await _context.Set<Ubicacion>()
                .Include(u => u.Sede)
                .ToListAsync();
        }

        public async Task<Ubicacion?> GetByIdWithSedeAsync(long id)
        {
            return await _context.Set<Ubicacion>()
                .Include(u => u.Sede)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Set<Ubicacion>().FindAsync(id);
            if (entity == null) return false;
            
            _context.Set<Ubicacion>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


