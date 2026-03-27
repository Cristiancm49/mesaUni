using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Security;

namespace MicroApi.Seguridad.Api.Controllers.Soporte
{
    [ApiController]
    [Route("api/intervenciones")]
    [Tags("IntervencionTecnica")]
    [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
    public class IntervencionTecnicaController : ControllerBase
    {
        private readonly IIntervencionTecnicaRepositoryExtended _repository;

        public IntervencionTecnicaController(IIntervencionTecnicaRepositoryExtended repository)
        {
            _repository = repository;
        }

        // ==================== FLUJO EN 3 FASES (RECOMENDADO) ====================

        /// <summary>
        /// FASE 1: Crear intervencion tecnica con SOLO diagnostico
        /// NO actualiza stock, NO inserta componentes ni consumibles
        /// Estado resultante: "Pendiente Aprobacion"
        /// </summary>
        [HttpPost("diagnostico")]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> CrearDiagnostico([FromBody] IntervencionDiagnosticoCreateDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos invalidos"));

                var intervencion = await _repository.SpIntervencionTecnicaCrearConDiagnosticoAsync(dto);

                return Ok(ApiResponseDto<dynamic>.SuccessResponse(
                    intervencion,
                    "Diagnostico registrado exitosamente. Pendiente de aprobacion administrativa."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al registrar diagnostico: {ex.Message}"));
            }
        }

        /// <summary>
        /// Actualizar el diagnostico de una intervencion existente
        /// Solo disponible cuando esta en estado "Pendiente Aprobacion"
        /// Util despues de un rechazo para corregir el diagnostico
        /// </summary>
        [HttpPut("{id}/diagnostico")]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> ActualizarDiagnostico(long id, [FromBody] IntervencionDiagnosticoUpdateDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (id != dto.IdIntervencionTecnica)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("El ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos invalidos"));

                var intervencion = await _repository.SpIntervencionTecnicaActualizarDiagnosticoAsync(dto);

                return Ok(ApiResponseDto<dynamic>.SuccessResponse(
                    intervencion,
                    "Diagnostico actualizado exitosamente. Puede volver a someter a aprobacion."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al actualizar diagnostico: {ex.Message}"));
            }
        }

        /// <summary>
        /// FASE 3: Ejecutar intervencion tecnica DESPUES de ser aprobada
        /// SI actualiza stock, SI inserta componentes y consumibles
        /// Prerequisito: La intervencion debe estar en estado "Aprobada"
        /// Estado resultante: "Pendiente Aprobacion Solucion"
        /// </summary>
        [HttpPost("{id}/ejecutar")]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> Ejecutar(long id, [FromBody] IntervencionEjecutarDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (id != dto.IdIntervencionTecnica)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("El ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos invalidos"));

                var resultado = await _repository.SpIntervencionTecnicaEjecutarAsync(dto);

                return Ok(ApiResponseDto<dynamic>.SuccessResponse(
                    resultado,
                    "Solucion registrada exitosamente. Pendiente de aprobacion administrativa final."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al ejecutar intervencion: {ex.Message}"));
            }
        }

        /// <summary>
        /// Corregir una solucion rechazada y reenviarla a revision administrativa final
        /// </summary>
        [HttpPut("{id}/solucion")]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> ActualizarSolucion(long id, [FromBody] IntervencionSolucionUpdateDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (id != dto.IdIntervencionTecnica)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("El ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos invalidos"));

                var intervencion = await _repository.SpIntervencionTecnicaActualizarSolucionAsync(dto);

                return Ok(ApiResponseDto<dynamic>.SuccessResponse(
                    intervencion,
                    "Solucion corregida y reenviada a revision administrativa final."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al actualizar solucion: {ex.Message}"));
            }
        }

        // ==================== ENDPOINTS LEGACY (DEPRECATED) ====================

        [HttpPost("crear-sp")]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> CrearConSP([FromBody] IntervencionTecnicaCreateDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos invalidos"));

                var intervencion = await _repository.SpIntervencionTecnicaCrearAsync(dto);
                return CreatedAtAction(nameof(CrearConSP), new { id = intervencion.Intervencion.Id },
                    ApiResponseDto<dynamic>.SuccessResponse(intervencion, "Intervencion tecnica creada exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al crear intervencion: {ex.Message}"));
            }
        }

        [HttpPut("{id}/actualizar-sp")]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> ActualizarConSP(long id, [FromBody] IntervencionTecnicaUpdateDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (id != dto.IdIntervencionTecnica)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos invalidos"));

                var intervencion = await _repository.SpIntervencionTecnicaActualizarAsync(dto);
                return Ok(ApiResponseDto<dynamic>.SuccessResponse(intervencion, "Intervencion tecnica actualizada exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al actualizar intervencion: {ex.Message}"));
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
