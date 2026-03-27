using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Data.Repositories
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
