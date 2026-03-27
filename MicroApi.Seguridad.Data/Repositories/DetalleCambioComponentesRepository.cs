using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class DetalleCambioComponentesRepository : GenericRepository<DetalleCambioComponentes>, IDetalleCambioComponentesRepository
    {
        public DetalleCambioComponentesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DetalleCambioComponentes>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(d => d.IntervencionTecnica)
                .Include(d => d.Componente)
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();
        }

        public async Task<DetalleCambioComponentes?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(d => d.IntervencionTecnica)
                .Include(d => d.Componente)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DetalleCambioComponentes>> GetByIntervencionIdAsync(long idIntervencion)
        {
            return await _dbSet
                .Include(d => d.Componente)
                .Where(d => d.IdIntervencionTecnica == idIntervencion)
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();
        }
    }
}











