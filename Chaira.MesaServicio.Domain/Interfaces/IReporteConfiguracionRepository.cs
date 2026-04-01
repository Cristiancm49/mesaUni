using Chaira.MesaServicio.Domain.DTOs.Reportes;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IReporteConfiguracionRepository
    {
        Task<ConfiguracionAuditReportDto> GetAuditoriaAsync(ConfiguracionAuditQueryDto query);
    }
}

