using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Application.Auth.Commands.Login;
using Chaira.MesaServicio.Application.Auth.Models;
using Chaira.MesaServicio.Domain.Security;

namespace Chaira.MesaServicio.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [Tags("Autenticacion")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email))
            {
                return BadRequest(new LoginResponseDto
                {
                    Success = false,
                    Message = "Email es requerido"
                });
            }

            var result = await _sender.Send(new LoginCommand(loginDto));

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpGet("validate")]
        [Authorize(Roles = AppRoles.Todos)]
        public IActionResult ValidateToken()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                      ?? User.FindFirst("sub")?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                     ?? User.FindFirst("email")?.Value;
            var name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value
                    ?? User.FindFirst("name")?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                success = true,
                message = "Token valido",
                usuario = new
                {
                    id = userId,
                    email,
                    nombre = name,
                    rol = role
                }
            });
        }
    }
}

