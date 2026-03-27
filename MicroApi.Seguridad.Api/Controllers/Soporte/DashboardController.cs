using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Security;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Interfaces;
namespace MicroApi.Seguridad.Api.Controllers.Soporte
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
        /// Obtener estadísticas completas de casos para el dashboard
        /// </summary>
        /// <remarks>
        /// Retorna 8 conjuntos de datos:
        /// 1. Resumen general (total casos, abiertos, resueltos, cerrados, sin asignar, tiempos promedio)
        /// 2. Casos por estado (con porcentajes)
        /// 3. Casos por prioridad (con casos abiertos y cerrados)
        /// 4. Casos por técnico (top 10 con métricas de rendimiento)
        /// 5. Casos por área técnica (con tiempos promedio)
        /// 6. Casos por tipo (con porcentajes)
        /// 7. Tendencia diaria (casos creados, resueltos y cerrados por día)
        /// 8. Estado SLA (casos en tiempo, próximos a vencer, vencidos)
        /// 
        /// Filtros opcionales:
        /// - FechaDesde/FechaHasta: Rango de fechas (por defecto últimos 30 días)
        /// - IdAreaTecnica: Filtrar por área técnica específica
        /// - IdTecnico: Filtrar por técnico específico
        /// </remarks>
        [HttpPost("estadisticas-casos")]
        public async Task<ActionResult<ApiResponseDto<DashboardEstadisticasDto>>> EstadisticasCasos([FromBody] DashboardEstadisticasRequestDto request)
        {
            try
            {
                var estadisticas = await _repository.SpDashboardEstadisticasCasosAsync(request);
                return Ok(ApiResponseDto<DashboardEstadisticasDto>.SuccessResponse(estadisticas, "Estadísticas obtenidas exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<DashboardEstadisticasDto>.FailResponse($"Error al obtener estadísticas: {ex.Message}"));
            }
        }
    }
}

