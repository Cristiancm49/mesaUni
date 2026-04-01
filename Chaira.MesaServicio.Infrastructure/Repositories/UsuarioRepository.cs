using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Usuario>> GetAllWithRolAsync()
        {
            return await _dbSet
                .Include(u => u.Rol)
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdWithRolAsync(long id)
        {
            return await _dbSet
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}













