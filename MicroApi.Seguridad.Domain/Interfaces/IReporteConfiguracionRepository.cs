using MicroApi.Seguridad.Domain.DTOs.Reportes;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IReporteConfiguracionRepository
    {
        Task<ConfiguracionAuditReportDto> GetAuditoriaAsync(ConfiguracionAuditQueryDto query);
    }
}
