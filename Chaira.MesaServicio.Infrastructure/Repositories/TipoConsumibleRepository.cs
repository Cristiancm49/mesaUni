using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class TipoConsumibleRepository : GenericRepository<TipoConsumible>, ITipoConsumibleRepository
    {
        public TipoConsumibleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TipoConsumible>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(t => t.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<TipoConsumible?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(t => t.EstadoGeneral)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}













