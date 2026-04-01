using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class SedeRepository : GenericRepository<Sede>, ISedeRepository
    {
        public SedeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Sede>> GetAllWithEstadoAsync()
        {
            return await _context.Set<Sede>()
                .Include(s => s.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<Sede?> GetByIdWithEstadoAsync(long id)
        {
            return await _context.Set<Sede>()
                .Include(s => s.EstadoGeneral)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}











