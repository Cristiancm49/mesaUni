using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class DetalleEncuestaRepository : GenericRepository<DetalleEncuesta>, IDetalleEncuestaRepository
    {
        public DetalleEncuestaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DetalleEncuesta>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(d => d.Encuesta)
                .Include(d => d.Pregunta)
                .Include(d => d.Respuesta)
                .OrderByDescending(d => d.FechaRegistro)
                .ToListAsync();
        }

        public async Task<DetalleEncuesta?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(d => d.Encuesta)
                .Include(d => d.Pregunta)
                .Include(d => d.Respuesta)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DetalleEncuesta>> GetByEncuestaIdAsync(long idEncuesta)
        {
            return await _dbSet
                .Include(d => d.Pregunta)
                .Include(d => d.Respuesta)
                .Where(d => d.IdEncuesta == idEncuesta)
                .OrderBy(d => d.Id)
                .ToListAsync();
        }
    }
}











