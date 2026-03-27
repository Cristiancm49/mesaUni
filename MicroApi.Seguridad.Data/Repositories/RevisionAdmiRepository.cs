using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Soporte;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class RevisionAdmiRepository : GenericRepository<RevisionAdmi>, IRevisionAdmiRepository
    {
        public RevisionAdmiRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RevisionAdmi>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(r => r.IntervencionTecnica)
                .OrderByDescending(r => r.FechaRegistro)
                .ToListAsync();
        }

        public async Task<RevisionAdmi?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(r => r.IntervencionTecnica)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RevisionAdmi?> GetByIntervencionIdAsync(long idIntervencion)
        {
            return await _dbSet
                .Include(r => r.IntervencionTecnica)
                .Where(r => r.IdIntervencionTecnica == idIntervencion)
                .OrderByDescending(r => r.FechaRegistro)
                .FirstOrDefaultAsync();
        }
    }
}











