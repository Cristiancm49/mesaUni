using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Security;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Interfaces;
namespace Chaira.MesaServicio.Api.Controllers.Inventario
{
    [ApiController]
    [Route("api/inventario/hojas-vida")]
    [Produces("application/json")]
    [Tags("HojaDeVidaActivo")]
    [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
    public class HojaDeVidaActivoController : ControllerBase
    {
        private readonly IHojaDeVidaActivoService _service;

        public HojaDeVidaActivoController(IHojaDeVidaActivoService service)
        {
            _service = service;
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

        [HttpGet("activo/{idActivo:long}")]
        public async Task<IActionResult> ObtenerPorActivo(long idActivo)
        {
            var result = await _service.GetByActivoIdAsync(idActivo);
            return Ok(result);
        }

        [HttpGet("caso/{idCaso:long}")]
        public async Task<IActionResult> ObtenerPorCaso(long idCaso)
        {
            var result = await _service.GetByCasoIdAsync(idCaso);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> ContarTotal()
        {
            var result = await _service.CountAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> Crear([FromBody] HojaDeVidaActivoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result);
        }
    }
}


