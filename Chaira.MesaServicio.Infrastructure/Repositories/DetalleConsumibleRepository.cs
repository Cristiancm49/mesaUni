using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class DetalleConsumibleRepository : GenericRepository<DetalleConsumible>, IDetalleConsumibleRepository
    {
        public DetalleConsumibleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DetalleConsumible>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(d => d.IntervencionTecnica)
                .Include(d => d.Consumible)
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();
        }

        public async Task<DetalleConsumible?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(d => d.IntervencionTecnica)
                .Include(d => d.Consumible)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DetalleConsumible>> GetByIntervencionIdAsync(long idIntervencion)
        {
            return await _dbSet
                .Include(d => d.Consumible)
                .Where(d => d.IdIntervencionTecnica == idIntervencion)
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();
        }
    }
}












