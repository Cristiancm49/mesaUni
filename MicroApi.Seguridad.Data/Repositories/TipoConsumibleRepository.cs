using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Data.Repositories
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












