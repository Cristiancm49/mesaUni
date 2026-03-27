using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IHojaDeVidaActivoRepository : IGenericRepository<HojaDeVidaActivo>
    {
        Task<IEnumerable<HojaDeVidaActivo>> GetAllWithRelationsAsync();
        Task<HojaDeVidaActivo?> GetByIdWithRelationsAsync(long id);
        Task<IEnumerable<HojaDeVidaActivo>> GetByActivoIdAsync(long idActivo);
        Task<IEnumerable<HojaDeVidaActivo>> GetByCasoIdAsync(long idCaso);
    }
}











