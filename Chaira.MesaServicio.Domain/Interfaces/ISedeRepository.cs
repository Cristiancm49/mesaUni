using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface ISedeRepository : IGenericRepository<Sede>
    {
        Task<IEnumerable<Sede>> GetAllWithEstadoAsync();
        Task<Sede?> GetByIdWithEstadoAsync(long id);
    }
}











