using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Catalogo;
using Chaira.MesaServicio.Domain.Security;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Interfaces;
namespace Chaira.MesaServicio.Api.Controllers.Catalogo
{
    [ApiController]
    [Route("api/catalogo/estados-consumible")]
    [Tags("EstadoConsumible")]
    [Authorize(Roles = AppRoles.Todos)]
    public class EstadoConsumibleController : ControllerBase
    {
        private readonly IEstadoConsumibleService _service;
        public EstadoConsumibleController(IEstadoConsumibleService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerPorId(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> ContarTotal() => Ok(await _service.CountAsync());

        [HttpPost]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Crear([FromBody] EstadoConsumibleCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.Success ? CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result) : BadRequest(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Actualizar(long id, [FromBody] EstadoConsumibleUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}


