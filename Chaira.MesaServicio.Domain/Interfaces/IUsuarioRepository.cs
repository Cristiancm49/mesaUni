using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<IEnumerable<Usuario>> GetAllWithRolAsync();
        Task<Usuario?> GetByIdWithRolAsync(long id);
        Task<Usuario?> GetByEmailAsync(string email);
    }
}













