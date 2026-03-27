using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class TrazabilidadCasoRepository : GenericRepository<TrazabilidadCaso>, ITrazabilidadCasoRepository
    {
        public TrazabilidadCasoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TrazabilidadCaso>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(t => t.Caso)
                .Include(t => t.EstadoCaso)
                .Include(t => t.AreaTecnica)
                .OrderByDescending(t => t.FechaEvento)
                .ToListAsync();
        }

        public async Task<TrazabilidadCaso?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(t => t.Caso)
                .Include(t => t.EstadoCaso)
                .Include(t => t.AreaTecnica)
                .Include(t => t.IntervencionesTecnicas)
                    .ThenInclude(i => i.TipoTrabajo)
                .Include(t => t.IntervencionesTecnicas)
                    .ThenInclude(i => i.EstadoIntervencion)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TrazabilidadCaso>> GetByCasoIdAsync(long idCaso)
        {
            return await _dbSet
                .Include(t => t.EstadoCaso)
                .Include(t => t.AreaTecnica)
                .Include(t => t.IntervencionesTecnicas)
                .Where(t => t.IdCaso == idCaso)
                .OrderByDescending(t => t.FechaEvento)
                .ToListAsync();
        }
    }
}











