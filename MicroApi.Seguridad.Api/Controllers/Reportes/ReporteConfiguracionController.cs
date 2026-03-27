using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Reportes;
using MicroApi.Seguridad.Domain.Security;
using MicroApi.Seguridad.Domain.Interfaces;

namespace MicroApi.Seguridad.Api.Controllers.Reportes
{
    [ApiController]
    [Route("api/reportes/configuracion")]
    [Tags("ReportesConfiguracion")]
    [Authorize(Roles = AppRoles.Administrador)]
    public class ReporteConfiguracionController : ControllerBase
    {
        private readonly IReporteConfiguracionRepository _repository;

        public ReporteConfiguracionController(IReporteConfiguracionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("auditoria")]
        public async Task<ActionResult<ApiResponseDto<ConfiguracionAuditReportDto>>> GetAuditoria([FromQuery] ConfiguracionAuditQueryDto query)
        {
            try
            {
                var reporte = await _repository.GetAuditoriaAsync(query);
                return Ok(ApiResponseDto<ConfiguracionAuditReportDto>.SuccessResponse(
                    reporte,
                    "Auditoria de configuracion obtenida"));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage = $"{errorMessage} {ex.InnerException.Message}";
                }

                if (errorMessage.Contains("auditoria.vwReporteConfiguracionBase", StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(ApiResponseDto<ConfiguracionAuditReportDto>.SuccessResponse(
                        new ConfiguracionAuditReportDto(),
                        "La vista de auditoria real aun no existe en la base de datos. Ejecuta el script reportes_configuracion_auditoria_views.sql para habilitar este reporte."));
                }

                return BadRequest(ApiResponseDto<ConfiguracionAuditReportDto>.FailResponse(
                    $"Error al obtener auditoria de configuracion: {ex.Message}"));
            }
        }
    }
}

