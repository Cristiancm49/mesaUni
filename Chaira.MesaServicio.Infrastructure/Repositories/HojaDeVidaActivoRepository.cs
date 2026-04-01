using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class HojaDeVidaActivoRepository : GenericRepository<HojaDeVidaActivo>, IHojaDeVidaActivoRepository
    {
        public HojaDeVidaActivoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<HojaDeVidaActivo>> GetAllWithRelationsAsync()
        {
            return await _dbSet
                .Include(h => h.Activo)
                .Include(h => h.UsuarioCreacion)
                .OrderByDescending(h => h.FechaRegistro)
                .ToListAsync();
        }

        public async Task<HojaDeVidaActivo?> GetByIdWithRelationsAsync(long id)
        {
            return await _dbSet
                .Include(h => h.Activo)
                .Include(h => h.UsuarioCreacion)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<HojaDeVidaActivo>> GetByActivoIdAsync(long idActivo)
        {
            return await _dbSet
                .Include(h => h.Activo)
                .Include(h => h.UsuarioCreacion)
                .Where(h => h.IdActivo == idActivo)
                .OrderByDescending(h => h.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<HojaDeVidaActivo>> GetByCasoIdAsync(long idCaso)
        {
            return await _dbSet
                .Include(h => h.Activo)
                .Include(h => h.UsuarioCreacion)
                .Where(h => h.IdCaso == idCaso)
                .OrderByDescending(h => h.FechaRegistro)
                .ToListAsync();
        }
    }
}












