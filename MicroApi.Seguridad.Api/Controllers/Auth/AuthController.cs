using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Domain.DTOs.Auth;
using MicroApi.Seguridad.Domain.Security;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Interfaces;
namespace MicroApi.Seguridad.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    [Tags("Autenticación")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login - Genera un token JWT para el usuario
        /// </summary>
        /// <remarks>
        /// Para desarrollo: usa el email de un usuario existente y cualquier password.
        /// El token generado tiene validez de 60 minutos por defecto.
        /// </remarks>
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

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Valida si el token actual es válido
        /// </summary>
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
                message = "Token válido",
                usuario = new
                {
                    id = userId,
                    email = email,
                    nombre = name,
                    rol = role
                }
            });
        }
    }
}

