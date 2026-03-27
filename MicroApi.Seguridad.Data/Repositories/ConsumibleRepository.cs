using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class ConsumibleRepository : GenericRepository<Consumible>, IConsumibleRepository
    {
        public ConsumibleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Consumible>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(c => c.Inventario)
                .Include(c => c.TipoConsumible)
                .Include(c => c.EstadoConsumible)
                .ToListAsync();
        }

        public async Task<Consumible?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(c => c.Inventario)
                .Include(c => c.TipoConsumible)
                .Include(c => c.EstadoConsumible)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Consumible>> SearchWithRelationsAsync(string query, int limit)
        {
            var term = query.Trim();
            var pattern = $"%{term}%";

            return await _dbSet
                .AsNoTracking()
                .Include(c => c.Inventario)
                .Include(c => c.TipoConsumible)
                .Include(c => c.EstadoConsumible)
                .Where(c =>
                    EF.Functions.Like(c.NombreConsumible, pattern) ||
                    (c.Marca != null && EF.Functions.Like(c.Marca, pattern)) ||
                    (c.Modelo != null && EF.Functions.Like(c.Modelo, pattern)) ||
                    (c.DescripcionTecnica != null && EF.Functions.Like(c.DescripcionTecnica, pattern)) ||
                    (c.Inventario != null && EF.Functions.Like(c.Inventario.NombreInventario, pattern)) ||
                    (c.TipoConsumible != null && EF.Functions.Like(c.TipoConsumible.NombreTipo, pattern)))
                .OrderBy(c => c.NombreConsumible)
                .Take(limit)
                .ToListAsync();
        }
    }
}












