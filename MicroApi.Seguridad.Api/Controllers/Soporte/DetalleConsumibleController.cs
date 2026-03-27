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
    [Route("api/detalles-consumibles")]
    [Tags("DetalleConsumible")]
    [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
    public class DetalleConsumibleController : ControllerBase
    {
        private readonly IDetalleConsumibleService _service;

        public DetalleConsumibleController(IDetalleConsumibleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<DetalleConsumibleDto>>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(ApiResponseDto<IEnumerable<DetalleConsumibleDto>>.SuccessResponse(items, "Detalles de consumibles obtenidos"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<DetalleConsumibleDto>>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseDto<DetalleConsumibleDto>.FailResponse("Detalle de consumible no encontrado"));
            return Ok(ApiResponseDto<DetalleConsumibleDto>.SuccessResponse(item, "Detalle de consumible obtenido"));
        }

        [HttpGet("intervencion/{idIntervencion}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<DetalleConsumibleDto>>>> GetByIntervencion(long idIntervencion)
        {
            var items = await _service.GetByIntervencionIdAsync(idIntervencion);
            return Ok(ApiResponseDto<IEnumerable<DetalleConsumibleDto>>.SuccessResponse(items, "Detalles de consumibles de la intervención obtenidos"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<DetalleConsumibleDto>>> Create([FromBody] DetalleConsumibleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDto<DetalleConsumibleDto>.FailResponse("Datos inválidos"));

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponseDto<DetalleConsumibleDto>.SuccessResponse(created, "Detalle de consumible registrado"));
        }
    }
}

