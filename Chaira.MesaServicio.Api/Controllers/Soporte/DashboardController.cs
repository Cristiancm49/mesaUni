using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Soporte;
using Chaira.MesaServicio.Domain.Security;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Interfaces;
namespace Chaira.MesaServicio.Api.Controllers.Soporte
{
    [ApiController]
    [Route("api/dashboard")]
    [Tags("Dashboard")]
    [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repository;

        public DashboardController(IDashboardRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtener estadÃ­sticas completas de casos para el dashboard
        /// </summary>
        /// <remarks>
        /// Retorna 8 conjuntos de datos:
        /// 1. Resumen general (total casos, abiertos, resueltos, cerrados, sin asignar, tiempos promedio)
        /// 2. Casos por estado (con porcentajes)
        /// 3. Casos por prioridad (con casos abiertos y cerrados)
        /// 4. Casos por tÃ©cnico (top 10 con mÃ©tricas de rendimiento)
        /// 5. Casos por Ã¡rea tÃ©cnica (con tiempos promedio)
        /// 6. Casos por tipo (con porcentajes)
        /// 7. Tendencia diaria (casos creados, resueltos y cerrados por dÃ­a)
        /// 8. Estado SLA (casos en tiempo, prÃ³ximos a vencer, vencidos)
        /// 
        /// Filtros opcionales:
        /// - FechaDesde/FechaHasta: Rango de fechas (por defecto Ãºltimos 30 dÃ­as)
        /// - IdAreaTecnica: Filtrar por Ã¡rea tÃ©cnica especÃ­fica
        /// - IdTecnico: Filtrar por tÃ©cnico especÃ­fico
        /// </remarks>
        [HttpPost("estadisticas-casos")]
        public async Task<ActionResult<ApiResponseDto<DashboardEstadisticasDto>>> EstadisticasCasos([FromBody] DashboardEstadisticasRequestDto request)
        {
            try
            {
                var estadisticas = await _repository.SpDashboardEstadisticasCasosAsync(request);
                return Ok(ApiResponseDto<DashboardEstadisticasDto>.SuccessResponse(estadisticas, "EstadÃ­sticas obtenidas exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<DashboardEstadisticasDto>.FailResponse($"Error al obtener estadÃ­sticas: {ex.Message}"));
            }
        }
    }
}


