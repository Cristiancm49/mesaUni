using Chaira.MesaServicio.Domain.DTOs.Reportes;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IReporteSoporteRepository
    {
        Task<IEnumerable<ReporteCasoDto>> GetCasosAsync();
        Task<IEnumerable<ReporteEncuestaDto>> GetEncuestasAsync();
    }
}

