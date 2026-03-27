using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Domain.DTOs.Acceso;
using MicroApi.Seguridad.Domain.Security;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Interfaces;
namespace MicroApi.Seguridad.Api.Controllers.Acceso
{
    [ApiController]
    [Route("api/acceso/usuarios")]
    [Produces("application/json")]
    [Tags("Usuario")]
    [Authorize(Roles = AppRoles.Todos)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ObtenerPorId(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ObtenerPorEmail(string email)
        {
            var result = await _service.GetByEmailAsync(email);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("count")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ContarTotal()
        {
            var result = await _service.CountAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Crear([FromBody] UsuarioCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Actualizar(long id, [FromBody] UsuarioUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}

