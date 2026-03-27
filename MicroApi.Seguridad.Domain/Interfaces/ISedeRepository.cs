using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ISedeRepository : IGenericRepository<Sede>
    {
        Task<IEnumerable<Sede>> GetAllWithEstadoAsync();
        Task<Sede?> GetByIdWithEstadoAsync(long id);
    }
}










