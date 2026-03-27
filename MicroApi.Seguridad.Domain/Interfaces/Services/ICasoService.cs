using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface ICasoService
    {
        Task<IEnumerable<CasoDto>> GetAllAsync();
        Task<CasoDto?> GetByIdAsync(long id);
        Task<CasoDetalleDto?> GetDetalleByIdAsync(long id);
        Task<IEnumerable<CasoDto>> GetByTecnicoAsync(long idTecnico);
        Task<IEnumerable<CasoDto>> GetByUsuarioReportaAsync(long idUsuarioReporta, MisIncidenciasFiltrosDto? filtros = null);
        Task<IEnumerable<CasoDto>> GetByEstadoAsync(long idEstadoCaso);
        Task<IEnumerable<CasoDto>> GetByFiltrosAsync(CasoFiltrosDto filtros);
        Task<CasoDto> CreateAsync(CasoCreateDto dto);
        Task<CasoDto?> UpdateAsync(CasoUpdateDto dto);
        Task<int> CountAsync();
    }
}
