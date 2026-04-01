using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Security;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Interfaces;
namespace Chaira.MesaServicio.Api.Controllers.Inventario
{
    [ApiController]
    [Route("api/inventario/consumibles")]
    [Produces("application/json")]
    [Tags("Consumible")]
    [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
    public class ConsumibleController : ControllerBase
    {
        private readonly IConsumibleService _service;

        public ConsumibleController(IConsumibleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string? q, [FromQuery] int limit = 20)
        {
            var result = await _service.SearchAsync(q, limit);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerPorId(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> ContarTotal()
        {
            var result = await _service.CountAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> Crear([FromBody] ConsumibleCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> Actualizar(long id, [FromBody] ConsumibleUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}


