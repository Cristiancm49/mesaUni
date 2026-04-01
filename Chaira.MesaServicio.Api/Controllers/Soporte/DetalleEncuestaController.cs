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
    [Route("api/detalles-encuesta")]
    [Tags("DetalleEncuesta")]
    [Authorize(Roles = AppRoles.Todos)]
    public class DetalleEncuestaController : ControllerBase
    {
        private readonly IDetalleEncuestaService _service;

        public DetalleEncuestaController(IDetalleEncuestaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<DetalleEncuestaDto>>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(ApiResponseDto<IEnumerable<DetalleEncuestaDto>>.SuccessResponse(items, "Detalles de encuesta obtenidos"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<DetalleEncuestaDto>>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseDto<DetalleEncuestaDto>.FailResponse("Detalle de encuesta no encontrado"));
            return Ok(ApiResponseDto<DetalleEncuestaDto>.SuccessResponse(item, "Detalle de encuesta obtenido"));
        }

        [HttpGet("encuesta/{idEncuesta}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<DetalleEncuestaDto>>>> GetByEncuesta(long idEncuesta)
        {
            var items = await _service.GetByEncuestaIdAsync(idEncuesta);
            return Ok(ApiResponseDto<IEnumerable<DetalleEncuestaDto>>.SuccessResponse(items, "Detalles de la encuesta obtenidos"));
        }
    }
}


