using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class ActivoRepository : GenericRepository<Activo>, IActivoRepository
    {
        public ActivoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Activo>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(a => a.CategoriaActivo)
                .Include(a => a.EstadoActivo)
                .Include(a => a.Inventario)
                .Include(a => a.Ubicacion)
                    .ThenInclude(u => u!.Sede)
                .Include(a => a.Responsable)
                .ToListAsync();
        }

        public async Task<Activo?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(a => a.CategoriaActivo)
                .Include(a => a.EstadoActivo)
                .Include(a => a.Inventario)
                .Include(a => a.Ubicacion)
                    .ThenInclude(u => u!.Sede)
                .Include(a => a.Responsable)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}



