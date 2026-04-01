using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Reportes;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Security;

namespace Chaira.MesaServicio.Api.Controllers.Reportes
{
    [ApiController]
    [Route("api/reportes/soporte")]
    [Tags("ReportesSoporte")]
    [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
    public class ReporteSoporteController : ControllerBase
    {
        private readonly IReporteSoporteRepository _repository;

        public ReporteSoporteController(IReporteSoporteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("casos")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ReporteCasoDto>>>> GetCasos()
        {
            try
            {
                var casos = await _repository.GetCasosAsync();
                return Ok(ApiResponseDto<IEnumerable<ReporteCasoDto>>.SuccessResponse(
                    casos,
                    "Reporte de casos obtenido correctamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<IEnumerable<ReporteCasoDto>>.FailResponse(
                    $"Error al obtener el reporte de casos: {ex.Message}"));
            }
        }

        [HttpGet("encuestas")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ReporteEncuestaDto>>>> GetEncuestas()
        {
            try
            {
                var encuestas = await _repository.GetEncuestasAsync();
                return Ok(ApiResponseDto<IEnumerable<ReporteEncuestaDto>>.SuccessResponse(
                    encuestas,
                    "Reporte de encuestas obtenido correctamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<IEnumerable<ReporteEncuestaDto>>.FailResponse(
                    $"Error al obtener el reporte de encuestas: {ex.Message}"));
            }
        }
    }
}

