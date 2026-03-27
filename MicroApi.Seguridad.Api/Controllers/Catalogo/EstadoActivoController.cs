using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.Security;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Interfaces;
namespace MicroApi.Seguridad.Api.Controllers.Catalogo
{
    [ApiController]
    [Route("api/catalogo/estados-activo")]
    [Tags("EstadoActivo")]
    [Authorize(Roles = AppRoles.Todos)]
    public class EstadoActivoController : ControllerBase
    {
        private readonly IEstadoActivoService _service;
        public EstadoActivoController(IEstadoActivoService service) => _service = service;

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
        public async Task<IActionResult> Crear([FromBody] EstadoActivoCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.Success ? CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result) : BadRequest(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Actualizar(long id, [FromBody] EstadoActivoUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}

