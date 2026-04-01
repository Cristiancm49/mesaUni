using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Security;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Interfaces;
namespace Chaira.MesaServicio.Api.Controllers.Inventario
{
    [ApiController]
    [Route("api/inventario/activos")]
    [Produces("application/json")]
    [Tags("Activo")]
    [Authorize(Roles = AppRoles.Todos)]
    public class ActivoController : ControllerBase
    {
        private readonly IActivoService _service;
        private readonly IHojaDeVidaActivoService _hojaDeVidaService;
        private readonly ICasoRepository _casoRepository;

        public ActivoController(
            IActivoService service,
            IHojaDeVidaActivoService hojaDeVidaService,
            ICasoRepository casoRepository)
        {
            _service = service;
            _hojaDeVidaService = hojaDeVidaService;
            _casoRepository = casoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerPorId(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id:long}/historial")]
        public async Task<IActionResult> ObtenerHistorial(long id)
        {
            var activo = await _service.GetByIdAsync(id);
            if (!activo.Success || activo.Data == null)
            {
                return NotFound(ApiResponseDto<ActivoHistorialDto>.FailResponse("Activo no encontrado"));
            }

            var hojaDeVida = await _hojaDeVidaService.GetByActivoIdAsync(id);
            var casos = await _casoRepository.GetByActivoAsync(id);

            var historialCasos = await Task.WhenAll(
                casos.Select(caso => _casoRepository.SpCasoObtenerHistorialAsync(caso.Id))
            );

            var result = new ActivoHistorialDto
            {
                Activo = activo.Data,
                HojaDeVida = hojaDeVida.Data?.ToList() ?? new List<HojaDeVidaActivoDto>(),
                Casos = historialCasos
                    .OrderByDescending(item => item.Caso?.FechaRegistro ?? DateTime.MinValue)
                    .ToList()
            };

            return Ok(ApiResponseDto<ActivoHistorialDto>.SuccessResponse(result, "Historial del activo obtenido"));
        }

        [HttpGet("count")]
        public async Task<IActionResult> ContarTotal()
        {
            var result = await _service.CountAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> Crear([FromBody] ActivoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> Actualizar(long id, [FromBody] ActivoUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new ApiResponseDto<ActivoDto>
                {
                    Success = false,
                    Message = "El ID de la ruta no coincide con el ID del cuerpo de la solicitud",
                    Data = null
                });

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseDto<ActivoDto>
                {
                    Success = false,
                    Message = "Datos invÃ¡lidos",
                    Data = null
                });

            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}


