using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class CanalIngresoRepository : GenericRepository<CanalIngreso>, ICanalIngresoRepository
    {
        public CanalIngresoRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<CanalIngreso>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<CanalIngreso?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

