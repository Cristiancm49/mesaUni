using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.ActualizarUsuario;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.CrearUsuario;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ContarUsuarios;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorEmail;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorId;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarios;
using Chaira.MesaServicio.Domain.Security;
namespace Chaira.MesaServicio.Api.Controllers.Acceso
{
    [ApiController]
    [Route("api/acceso/usuarios")]
    [Produces("application/json")]
    [Tags("Usuario")]
    [Authorize(Roles = AppRoles.Todos)]
    public class UsuarioController : ControllerBase
    {
        private readonly ISender _sender;

        public UsuarioController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _sender.Send(new ObtenerUsuariosQuery());
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ObtenerPorId(long id)
        {
            var result = await _sender.Send(new ObtenerUsuarioPorIdQuery(id));
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ObtenerPorEmail(string email)
        {
            var result = await _sender.Send(new ObtenerUsuarioPorEmailQuery(email));
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("count")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<IActionResult> ContarTotal()
        {
            var result = await _sender.Send(new ContarUsuariosQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Crear([FromBody] UsuarioCreateDto dto)
        {
            var result = await _sender.Send(new CrearUsuarioCommand(dto));
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Actualizar(long id, [FromBody] UsuarioUpdateDto dto)
        {
            dto.Id = id;
            var result = await _sender.Send(new ActualizarUsuarioCommand(id, dto));
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}


