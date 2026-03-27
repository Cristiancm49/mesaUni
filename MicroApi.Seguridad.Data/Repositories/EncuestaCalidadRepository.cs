using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class EncuestaCalidadRepository : GenericRepository<EncuestaCalidad>, IEncuestaCalidadRepository
    {
        public EncuestaCalidadRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EncuestaCalidad>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(e => e.Caso)
                .Include(e => e.Detalles)
                    .ThenInclude(d => d.Pregunta)
                .Include(e => e.Detalles)
                    .ThenInclude(d => d.Respuesta)
                .OrderByDescending(e => e.FechaEncuesta)
                .ToListAsync();
        }

        public async Task<EncuestaCalidad?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(e => e.Caso)
                .Include(e => e.Detalles)
                    .ThenInclude(d => d.Pregunta)
                .Include(e => e.Detalles)
                    .ThenInclude(d => d.Respuesta)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<EncuestaCalidad>> GetByCasoIdAsync(long idCaso)
        {
            return await _dbSet
                .Include(e => e.Detalles)
                    .ThenInclude(d => d.Pregunta)
                .Include(e => e.Detalles)
                    .ThenInclude(d => d.Respuesta)
                .Where(e => e.IdCaso == idCaso)
                .OrderByDescending(e => e.FechaEncuesta)
                .ToListAsync();
        }
    }
}











