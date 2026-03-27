using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Security;
using MicroApi.Seguridad.Domain.Interfaces;
namespace MicroApi.Seguridad.Api.Controllers.Soporte
{
    [ApiController]
    [Route("api/casos")]
    [Tags("Caso")]
    [Authorize(Roles = AppRoles.Todos)]
    public class CasoController : ControllerBase
    {
        private readonly ICasoService _service;
        private readonly ICasoRepository _repository;

        public CasoController(ICasoService service, ICasoRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CasoDto>>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(ApiResponseDto<IEnumerable<CasoDto>>.SuccessResponse(items, "Casos obtenidos"));
        }

        [HttpGet("mis-incidencias")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CasoDto>>>> GetMisIncidencias([FromQuery] MisIncidenciasFiltrosDto filtros)
        {
            var userId = ResolveUserId(User);
            if (!userId.HasValue)
            {
                return Unauthorized(ApiResponseDto<IEnumerable<CasoDto>>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));
            }

            var items = await _service.GetByUsuarioReportaAsync(userId.Value, filtros);
            return Ok(ApiResponseDto<IEnumerable<CasoDto>>.SuccessResponse(items, "Mis incidencias obtenidas"));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponseDto<CasoDetalleDto>>> GetById(long id)
        {
            var item = await _service.GetDetalleByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseDto<CasoDetalleDto>.FailResponse("Caso no encontrado"));
            return Ok(ApiResponseDto<CasoDetalleDto>.SuccessResponse(item, "Caso obtenido"));
        }

        [HttpGet("tecnico/{idTecnico}")]
        [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CasoDto>>>> GetByTecnico(long idTecnico)
        {
            var items = await _service.GetByTecnicoAsync(idTecnico);
            return Ok(ApiResponseDto<IEnumerable<CasoDto>>.SuccessResponse(items, "Casos del técnico obtenidos"));
        }

        [HttpGet("resumen-tecnicos")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<TecnicoIncidenciasResumenDto>>>> GetResumenTecnicos()
        {
            var items = await _repository.GetResumenTecnicosAsync();
            return Ok(ApiResponseDto<IEnumerable<TecnicoIncidenciasResumenDto>>.SuccessResponse(items, "Resumen operativo de tecnicos obtenido"));
        }

        [HttpGet("estado/{idEstado}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CasoDto>>>> GetByEstado(long idEstado)
        {
            var items = await _service.GetByEstadoAsync(idEstado);
            return Ok(ApiResponseDto<IEnumerable<CasoDto>>.SuccessResponse(items, "Casos por estado obtenidos"));
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<CasoDto>>> Update(long id, [FromBody] CasoUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponseDto<CasoDto>.FailResponse("ID no coincide"));

            var updated = await _service.UpdateAsync(dto);
            if (updated == null)
                return NotFound(ApiResponseDto<CasoDto>.FailResponse("Caso no encontrado"));

            return Ok(ApiResponseDto<CasoDto>.SuccessResponse(updated, "Caso actualizado"));
        }

        // ==================== ENDPOINTS CON STORED PROCEDURES ====================

        [HttpPost("crear-sp")]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> CrearConSP([FromBody] CasoCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos inválidos"));

                var caso = await _repository.SpCasoCrearAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = caso.Id },
                    ApiResponseDto<dynamic>.SuccessResponse(caso, "Caso creado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al crear caso: {ex.Message}"));
            }
        }

        [HttpPost("{id:long}/cambiar-estado")]
        [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> CambiarEstado(long id, [FromBody] CasoCambiarEstadoDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (id != dto.IdCaso)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos inválidos"));

                var caso = await _repository.SpCasoCambiarEstadoAsync(dto);
                return Ok(ApiResponseDto<dynamic>.SuccessResponse(caso, "Estado del caso actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al cambiar estado: {ex.Message}"));
            }
        }

        [HttpPost("{id:long}/asignar-tecnico")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> AsignarTecnico(long id, [FromBody] CasoAsignarTecnicoDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (id != dto.IdCaso)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos inválidos"));

                var caso = await _repository.SpCasoAsignarTecnicoAsync(dto);
                return Ok(ApiResponseDto<dynamic>.SuccessResponse(caso, "Técnico asignado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al asignar técnico: {ex.Message}"));
            }
        }

        [HttpPost("{id:long}/escalar")]
        [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> Escalar(long id, [FromBody] CasoEscalarDto dto)
        {
            try
            {
                var userId = ResolveUserId(User);
                if (!userId.HasValue)
                    return Unauthorized(ApiResponseDto<dynamic>.FailResponse("No se pudo resolver el usuario autenticado desde el token."));

                dto.IdUsuarioAccion = userId.Value;

                if (id != dto.IdCaso)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos inválidos"));

                var caso = await _repository.SpCasoEscalarAsync(dto);
                return Ok(ApiResponseDto<dynamic>.SuccessResponse(caso, "Caso escalado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al escalar caso: {ex.Message}"));
            }
        }

        [HttpPost("{id:long}/asignar-activo")]
        [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> AsignarActivo(long id, [FromBody] CasoAsignarActivoDto dto)
        {
            try
            {
                if (id != dto.IdCaso)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("ID no coincide"));

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponseDto<dynamic>.FailResponse("Datos inválidos"));

                var caso = await _repository.SpCasoAsignarActivoAsync(dto);
                return Ok(ApiResponseDto<dynamic>.SuccessResponse(caso, "Activo asignado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error al asignar activo: {ex.Message}"));
            }
        }

        [HttpPost("buscar-avanzada")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<dynamic>>> BuscarAvanzada(
            [FromQuery] long? idEstadoCaso,
            [FromQuery] long? idTecnicoAsignado,
            [FromQuery] long? idAreaTecnica,
            [FromQuery] long? idPrioridad,
            [FromQuery] long? idUsuarioReporta,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string? textoBusqueda,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string orderBy = "FechaRegistro",
            [FromQuery] string orderDirection = "DESC")
        {
            try
            {
                var (totalRegistros, casos) = await _repository.SpCasoObtenerPorFiltrosAsync(
                    idEstadoCaso, idTecnicoAsignado, idAreaTecnica, idPrioridad,
                    idUsuarioReporta, fechaDesde, fechaHasta, textoBusqueda,
                    page, pageSize, orderBy, orderDirection);

                var result = new
                {
                    TotalRegistros = totalRegistros,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalRegistros / (double)pageSize),
                    Casos = casos
                };

                return Ok(ApiResponseDto<dynamic>.SuccessResponse(result, "Búsqueda completada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<dynamic>.FailResponse($"Error en búsqueda: {ex.Message}"));
            }
        }

        [HttpGet("{id:long}/historial-completo")]
        public async Task<ActionResult<ApiResponseDto<CasoHistorialCompletoDto>>> ObtenerHistorialCompleto(long id)
        {
            try
            {
                var historial = await _repository.SpCasoObtenerHistorialAsync(id);
                return Ok(ApiResponseDto<CasoHistorialCompletoDto>.SuccessResponse(historial, "Historial obtenido exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<CasoHistorialCompletoDto>.FailResponse($"Error al obtener historial: {ex.Message}"));
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

