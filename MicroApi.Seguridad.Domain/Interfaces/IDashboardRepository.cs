using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardEstadisticasDto> SpDashboardEstadisticasCasosAsync(DashboardEstadisticasRequestDto request);
    }
}



