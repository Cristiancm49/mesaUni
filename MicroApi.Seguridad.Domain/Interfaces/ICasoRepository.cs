using MicroApi.Seguridad.Domain.Models.Soporte;
using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface ICasoRepository
    {
        Task<IEnumerable<Caso>> GetAllAsync();
        Task<Caso?> GetByIdAsync(long id);
        Task<IEnumerable<Caso>> GetByActivoAsync(long idActivo);
        Task<IEnumerable<Caso>> GetByTecnicoAsync(long idTecnico);
        Task<IEnumerable<Caso>> GetByUsuarioReportaAsync(long idUsuarioReporta);
        Task<IEnumerable<Caso>> GetByEstadoAsync(long idEstadoCaso);
        Task<IEnumerable<Caso>> GetByFiltrosAsync(long? idEstadoCaso, long? idTecnico, long? idAreaTecnica, DateTime? fechaDesde, DateTime? fechaHasta);
        Task<Caso> CreateAsync(Caso caso);
        Task<Caso> UpdateAsync(Caso caso);
        Task<bool> ExistsAsync(long id);
        Task<int> CountAsync();
        Task<IEnumerable<Caso>> GetPagedAsync(int page, int pageSize);

        // Métodos para Stored Procedures
        Task<dynamic> SpCasoCrearAsync(CasoCreateDto dto);
        Task<dynamic> SpCasoCambiarEstadoAsync(CasoCambiarEstadoDto dto);
        Task<dynamic> SpCasoAsignarTecnicoAsync(CasoAsignarTecnicoDto dto);
        Task<dynamic> SpCasoEscalarAsync(CasoEscalarDto dto);
        Task<dynamic> SpCasoAsignarActivoAsync(CasoAsignarActivoDto dto);
        Task<(int TotalRegistros, List<dynamic> Casos)> SpCasoObtenerPorFiltrosAsync(
            long? idEstadoCaso, long? idTecnicoAsignado, long? idAreaTecnica, long? idPrioridad,
            long? idUsuarioReporta, DateTime? fechaDesde, DateTime? fechaHasta, string? textoBusqueda,
            int page, int pageSize, string orderBy, string orderDirection);
        Task<CasoHistorialCompletoDto> SpCasoObtenerHistorialAsync(long idCaso);
        Task<IEnumerable<TecnicoIncidenciasResumenDto>> GetResumenTecnicosAsync();
    }
}
