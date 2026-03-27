using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class AreaTecnicaRepository : GenericRepository<AreaTecnica>, IAreaTecnicaRepository
    {
        public AreaTecnicaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AreaTecnica>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(a => a.EstadoGeneral)
                .Include(a => a.Encargado)
                .ToListAsync();
        }

        public async Task<AreaTecnica?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(a => a.EstadoGeneral)
                .Include(a => a.Encargado)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
