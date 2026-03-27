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
    [Route("api/trazabilidades")]
    [Tags("TrazabilidadCaso")]
    [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
    public class TrazabilidadCasoController : ControllerBase
    {
        private readonly ITrazabilidadCasoService _service;

        public TrazabilidadCasoController(ITrazabilidadCasoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TrazabilidadCasoDto>>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(ApiResponseDto<IEnumerable<TrazabilidadCasoDto>>.SuccessResponse(items, "Trazabilidades obtenidas"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<TrazabilidadCasoDto>>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseDto<TrazabilidadCasoDto>.FailResponse("Trazabilidad no encontrada"));
            return Ok(ApiResponseDto<TrazabilidadCasoDto>.SuccessResponse(item, "Trazabilidad obtenida"));
        }

        [HttpGet("caso/{idCaso}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TrazabilidadCasoDto>>>> GetByCaso(long idCaso)
        {
            var items = await _service.GetByCasoIdAsync(idCaso);
            return Ok(ApiResponseDto<IEnumerable<TrazabilidadCasoDto>>.SuccessResponse(items, "Historial del caso obtenido"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<TrazabilidadCasoDto>>> Create([FromBody] TrazabilidadCasoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponseDto<TrazabilidadCasoDto>.FailResponse("Datos inválidos"));

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponseDto<TrazabilidadCasoDto>.SuccessResponse(created, "Trazabilidad creada"));
        }
    }
}

