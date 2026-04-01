using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class PrioridadRepository : GenericRepository<Prioridad>, IPrioridadRepository
    {
        public PrioridadRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Prioridad>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<Prioridad?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

