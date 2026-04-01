using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Soporte;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Security;

namespace Chaira.MesaServicio.Api.Controllers.Soporte
{
    [ApiController]
    [Route("api/revisiones")]
    [Tags("RevisionAdmi")]
    [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
    public class RevisionAdmiController : ControllerBase
    {
        private readonly IRevisionAdmiRepositoryExtended _repository;

        public RevisionAdmiController(IRevisionAdmiRepositoryExtended repository)
        {
            _repository = repository;
        }

        [HttpGet("bandeja")]
        public async Task<ActionResult<ApiResponseDto<RevisionAdmiQueueDto>>> ObtenerBandeja()
        {
            try
            {
                var data = await _repository.SpRevisionAdmiObtenerBandejaAsync();
                return Ok(ApiResponseDto<RevisionAdmiQueueDto>.SuccessResponse(data, "Bandeja de revision cargada exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<RevisionAdmiQueueDto>.FailResponse($"Error al obtener la bandeja de revision: {ex.Message}"));
            }
        }

        /// <summary>
        /// Aprobar o rechazar una revision administrativa de diagnostico o solucion
        /// </summary>
        [HttpPost("procesar")]
        public async Task<ActionResult<ApiResponseDto<RevisionAdmiResponseDto>>> Procesar([FromBody] RevisionAdmiCreateDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<RevisionAdmiResponseDto>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioCreacion = userId.Value;

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<RevisionAdmiResponseDto>.FailResponse("Datos invalidos"));

                var revision = await _repository.SpRevisionAdmiProcesarAsync(dto);
                return CreatedAtAction(nameof(Procesar), new { id = revision.Id },
                    ApiResponseDto<RevisionAdmiResponseDto>.SuccessResponse(revision, "Revision administrativa procesada exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<RevisionAdmiResponseDto>.FailResponse($"Error al procesar revision: {ex.Message}"));
            }
        }

        private static long? ResolveUserId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                        ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);

            return long.TryParse(value, out var parsed) ? parsed : null;
        }
    }
}

