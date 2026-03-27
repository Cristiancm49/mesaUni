using MicroApi.Seguridad.Domain.Models.Acceso;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<IEnumerable<Usuario>> GetAllWithRolAsync();
        Task<Usuario?> GetByIdWithRolAsync(long id);
        Task<Usuario?> GetByEmailAsync(string email);
    }
}












