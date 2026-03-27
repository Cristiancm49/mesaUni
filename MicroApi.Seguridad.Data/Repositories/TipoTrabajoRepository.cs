using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Data.Repositories
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
