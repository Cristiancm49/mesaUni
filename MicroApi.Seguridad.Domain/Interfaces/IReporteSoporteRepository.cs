using MicroApi.Seguridad.Domain.DTOs.Reportes;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IReporteSoporteRepository
    {
        Task<IEnumerable<ReporteCasoDto>> GetCasosAsync();
        Task<IEnumerable<ReporteEncuestaDto>> GetEncuestasAsync();
    }
}
