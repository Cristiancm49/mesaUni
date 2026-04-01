using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Soporte;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class IntervencionTecnicaRepository : GenericRepository<IntervencionTecnica>, IIntervencionTecnicaRepository
    {
        public IntervencionTecnicaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<IntervencionTecnica>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(i => i.TrazabilidadCaso)
                .Include(i => i.TipoTrabajo)
                .Include(i => i.EstadoIntervencion)
                .OrderByDescending(i => i.FechaInicio)
                .ToListAsync();
        }

        public async Task<IntervencionTecnica?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(i => i.TrazabilidadCaso)
                    .ThenInclude(t => t.Caso)
                .Include(i => i.TipoTrabajo)
                .Include(i => i.EstadoIntervencion)
                .Include(i => i.CambiosComponentes)
                    .ThenInclude(c => c.Componente)
                .Include(i => i.DetallesConsumibles)
                    .ThenInclude(d => d.Consumible)
                .Include(i => i.RevisionesAdmi)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<IntervencionTecnica>> GetByTrazabilidadIdAsync(long idTrazabilidad)
        {
            return await _dbSet
                .Include(i => i.TipoTrabajo)
                .Include(i => i.EstadoIntervencion)
                .Where(i => i.IdTrazabilidadCaso == idTrazabilidad)
                .OrderByDescending(i => i.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<IntervencionTecnica>> GetByCasoIdAsync(long idCaso)
        {
            return await _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(i => i.TrazabilidadCaso)
                    .ThenInclude(t => t.Caso)
                .Include(i => i.TipoTrabajo)
                .Include(i => i.EstadoIntervencion)
                .Include(i => i.CambiosComponentes)
                    .ThenInclude(c => c.Componente)
                .Include(i => i.DetallesConsumibles)
                    .ThenInclude(d => d.Consumible)
                .Include(i => i.RevisionesAdmi)
                .Where(i => i.TrazabilidadCaso.IdCaso == idCaso)
                .OrderByDescending(i => i.FechaInicio)
                .ToListAsync();
        }
    }
}

