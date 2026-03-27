using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class ComponenteRepository : GenericRepository<Componente>, IComponenteRepository
    {
        public ComponenteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Componente>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(c => c.Inventario)
                .Include(c => c.EstadoGeneral)
                .ToListAsync();
        }

        public async Task<Componente?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(c => c.Inventario)
                .Include(c => c.EstadoGeneral)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Componente>> SearchWithRelationsAsync(string query, int limit)
        {
            var term = query.Trim();
            var pattern = $"%{term}%";

            return await _dbSet
                .AsNoTracking()
                .Include(c => c.Inventario)
                .Include(c => c.EstadoGeneral)
                .Where(c =>
                    EF.Functions.Like(c.NombreComponente, pattern) ||
                    (c.Marca != null && EF.Functions.Like(c.Marca, pattern)) ||
                    (c.Modelo != null && EF.Functions.Like(c.Modelo, pattern)) ||
                    (c.Descripcion != null && EF.Functions.Like(c.Descripcion, pattern)) ||
                    (c.Inventario != null && EF.Functions.Like(c.Inventario.NombreInventario, pattern)))
                .OrderBy(c => c.NombreComponente)
                .Take(limit)
                .ToListAsync();
        }
    }
}












