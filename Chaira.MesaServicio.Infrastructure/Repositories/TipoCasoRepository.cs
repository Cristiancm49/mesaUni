using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class TipoCasoRepository : GenericRepository<TipoCaso>, ITipoCasoRepository
    {
        public TipoCasoRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<TipoCaso>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<TipoCaso?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(x => x.EstadoGeneral)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

