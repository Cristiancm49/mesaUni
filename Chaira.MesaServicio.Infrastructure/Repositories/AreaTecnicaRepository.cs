using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
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

