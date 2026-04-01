using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class TipoTrabajoRepository : GenericRepository<TipoTrabajo>, ITipoTrabajoRepository
    {
        public TipoTrabajoRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<TipoTrabajo>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<TipoTrabajo?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

