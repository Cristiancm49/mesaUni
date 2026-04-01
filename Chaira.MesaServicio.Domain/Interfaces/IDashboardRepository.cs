using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardEstadisticasDto> SpDashboardEstadisticasCasosAsync(DashboardEstadisticasRequestDto request);
    }
}




