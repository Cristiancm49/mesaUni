using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Api.Models;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Security;

namespace MicroApi.Seguridad.Api.Controllers.Soporte
{
    [ApiController]
    [Route("api/evidencias")]
    [Tags("Evidencia")]
    [Authorize(Roles = AppRoles.Todos)]
    public class EvidenciaController : ControllerBase
    {
        private readonly IEvidenciaService _service;

        public EvidenciaController(IEvidenciaService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> Upload(
            [FromForm] EvidenciaUploadMultipleRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.FailResponse("Datos invalidos para carga de evidencia."));
                }

                if (request.Files == null || request.Files.Length == 0)
                {
                    return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.FailResponse("Debe adjuntar al menos un archivo."));
                }

                var userId = request.IdUsuarioCarga ?? ResolveUserId(User);
                var metadata = new EvidenciaUploadMetadataDto
                {
                    Modulo = request.Modulo,
                    Entidad = request.Entidad,
                    EntidadId = request.EntidadId,
                    TipoEvidencia = request.TipoEvidencia,
                    Descripcion = request.Descripcion,
                    IdCaso = request.IdCaso,
                    IdIntervencionTecnica = request.IdIntervencionTecnica,
                    IdUsuarioCarga = userId
                };

                var evidencias = await UploadManyAsync(request.Files, metadata, cancellationToken);

                return Ok(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.SuccessResponse(evidencias, "Evidencias cargadas correctamente."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.FailResponse($"Error al cargar evidencias: {ex.Message}"));
            }
        }

        [HttpPost("soporte/casos/{idCaso:long}")]
        [Consumes("multipart/form-data")]
        public Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadCaso(
            long idCaso,
            [FromForm] EvidenciaUploadFilesRequestDto request,
            CancellationToken cancellationToken)
            => UploadByContext(
                modulo: "soporte",
                entidad: "caso",
                entidadId: idCaso,
                defaultTipoEvidencia: "creacion",
                idCaso: idCaso,
                idIntervencionTecnica: null,
                request: request,
                cancellationToken: cancellationToken);

        [HttpPost("soporte/intervenciones/{idIntervencionTecnica:long}/diagnostico")]
        [Consumes("multipart/form-data")]
        public Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadDiagnosticoIntervencion(
            long idIntervencionTecnica,
            [FromForm] EvidenciaUploadFilesRequestDto request,
            [FromQuery] long? idCaso,
            CancellationToken cancellationToken)
            => UploadByContext(
                modulo: "soporte",
                entidad: "intervencion",
                entidadId: idIntervencionTecnica,
                defaultTipoEvidencia: "diagnostico",
                idCaso: idCaso,
                idIntervencionTecnica: idIntervencionTecnica,
                request: request,
                cancellationToken: cancellationToken);

        [HttpPost("soporte/intervenciones/{idIntervencionTecnica:long}/ejecucion")]
        [Consumes("multipart/form-data")]
        public Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadEjecucionIntervencion(
            long idIntervencionTecnica,
            [FromForm] EvidenciaUploadFilesRequestDto request,
            [FromQuery] long? idCaso,
            CancellationToken cancellationToken)
            => UploadByContext(
                modulo: "soporte",
                entidad: "intervencion",
                entidadId: idIntervencionTecnica,
                defaultTipoEvidencia: "ejecucion",
                idCaso: idCaso,
                idIntervencionTecnica: idIntervencionTecnica,
                request: request,
                cancellationToken: cancellationToken);

        [HttpPost("soporte/intervenciones/{idIntervencionTecnica:long}/revision")]
        [Consumes("multipart/form-data")]
        public Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadRevisionIntervencion(
            long idIntervencionTecnica,
            [FromForm] EvidenciaUploadFilesRequestDto request,
            [FromQuery] long? idCaso,
            [FromQuery] string? tipoRevision,
            CancellationToken cancellationToken)
            => UploadByContext(
                modulo: "soporte",
                entidad: "intervencion",
                entidadId: idIntervencionTecnica,
                defaultTipoEvidencia: string.Equals(tipoRevision, "SOLUCION", StringComparison.OrdinalIgnoreCase)
                    ? "revision_solucion"
                    : "revision_diagnostico",
                idCaso: idCaso,
                idIntervencionTecnica: idIntervencionTecnica,
                request: request,
                cancellationToken: cancellationToken);

        [HttpPost("inventario/activos/{idActivo:long}/imagen")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadImagenActivo(
            long idActivo,
            [FromForm] EvidenciaUploadFilesRequestDto request,
            CancellationToken cancellationToken)
            => UploadByContext(
                modulo: "inventario",
                entidad: "activo",
                entidadId: idActivo,
                defaultTipoEvidencia: "imagen",
                idCaso: null,
                idIntervencionTecnica: null,
                request: request,
                cancellationToken: cancellationToken);

        [HttpPost("inventario/componentes/{idComponente:long}/imagen")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadImagenComponente(
            long idComponente,
            [FromForm] EvidenciaUploadFilesRequestDto request,
            CancellationToken cancellationToken)
            => UploadByContext(
                modulo: "inventario",
                entidad: "componente",
                entidadId: idComponente,
                defaultTipoEvidencia: "imagen",
                idCaso: null,
                idIntervencionTecnica: null,
                request: request,
                cancellationToken: cancellationToken);

        [HttpPost("inventario/consumibles/{idConsumible:long}/imagen")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadImagenConsumible(
            long idConsumible,
            [FromForm] EvidenciaUploadFilesRequestDto request,
            CancellationToken cancellationToken)
            => UploadByContext(
                modulo: "inventario",
                entidad: "consumible",
                entidadId: idConsumible,
                defaultTipoEvidencia: "imagen",
                idCaso: null,
                idIntervencionTecnica: null,
                request: request,
                cancellationToken: cancellationToken);

        [HttpGet("{modulo}/{entidad}/{entidadId:long}")]
        public async Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> GetByEntidad(
            string modulo,
            string entidad,
            long entidadId,
            [FromQuery] string? tipoEvidencia,
            CancellationToken cancellationToken)
        {
            try
            {
                var evidencias = await _service.GetByEntidadAsync(modulo, entidad, entidadId, tipoEvidencia, cancellationToken);
                return Ok(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.SuccessResponse(evidencias, "Evidencias obtenidas."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.FailResponse($"Error al obtener evidencias: {ex.Message}"));
            }
        }

        [HttpPost("{modulo}/{entidad}/batch")]
        public async Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciasPorEntidadDto>>>> GetByEntidades(
            string modulo,
            string entidad,
            [FromBody] EvidenciasBatchQueryDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciasPorEntidadDto>>.FailResponse("Debe enviar al menos un id de entidad."));
                }

                var result = await _service.GetByEntidadesAsync(modulo, entidad, request.EntidadIds, cancellationToken);
                return Ok(ApiResponseDto<IReadOnlyList<EvidenciasPorEntidadDto>>.SuccessResponse(result, "Evidencias agrupadas obtenidas."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciasPorEntidadDto>>.FailResponse($"Error en consulta batch: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<EvidenciaDto>>> GetById(string id, CancellationToken cancellationToken)
        {
            var evidencia = await _service.GetByIdAsync(id, cancellationToken);
            if (evidencia == null)
            {
                return NotFound(ApiResponseDto<EvidenciaDto>.FailResponse("Evidencia no encontrada."));
            }

            return Ok(ApiResponseDto<EvidenciaDto>.SuccessResponse(evidencia, "Evidencia obtenida."));
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(string id, CancellationToken cancellationToken)
        {
            var file = await _service.DownloadAsync(id, cancellationToken);
            if (file == null)
            {
                return NotFound(ApiResponseDto<string>.FailResponse("Archivo no encontrado."));
            }

            return File(file.Value.Stream, file.Value.ContentType, file.Value.FileName);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(string id, CancellationToken cancellationToken)
        {
            var userId = ResolveUserId(User);
            var deleted = await _service.SoftDeleteAsync(id, userId, cancellationToken);
            if (!deleted)
            {
                return NotFound(ApiResponseDto<bool>.FailResponse("No se pudo eliminar la evidencia."));
            }

            return Ok(ApiResponseDto<bool>.SuccessResponse(true, "Evidencia eliminada logicamente."));
        }

        private static long? ResolveUserId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                        ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);

            return long.TryParse(value, out var parsed) ? parsed : null;
        }

        private async Task<ActionResult<ApiResponseDto<IReadOnlyList<EvidenciaDto>>>> UploadByContext(
            string modulo,
            string entidad,
            long entidadId,
            string defaultTipoEvidencia,
            long? idCaso,
            long? idIntervencionTecnica,
            EvidenciaUploadFilesRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.FailResponse("Datos invalidos para carga de evidencia."));
                }

                if (request.Files == null || request.Files.Length == 0)
                {
                    return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.FailResponse("Debe adjuntar al menos un archivo."));
                }

                var userId = request.IdUsuarioCarga ?? ResolveUserId(User);
                var metadata = new EvidenciaUploadMetadataDto
                {
                    Modulo = modulo,
                    Entidad = entidad,
                    EntidadId = entidadId,
                    TipoEvidencia = string.IsNullOrWhiteSpace(request.TipoEvidencia) ? defaultTipoEvidencia : request.TipoEvidencia,
                    Descripcion = request.Descripcion,
                    IdCaso = idCaso,
                    IdIntervencionTecnica = idIntervencionTecnica,
                    IdUsuarioCarga = userId
                };

                var evidencias = await UploadManyAsync(request.Files, metadata, cancellationToken);
                return Ok(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.SuccessResponse(evidencias, "Evidencias cargadas correctamente."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<IReadOnlyList<EvidenciaDto>>.FailResponse($"Error al cargar evidencias: {ex.Message}"));
            }
        }

        private async Task<IReadOnlyList<EvidenciaDto>> UploadManyAsync(
            IEnumerable<IFormFile> files,
            EvidenciaUploadMetadataDto metadata,
            CancellationToken cancellationToken)
        {
            var result = new List<EvidenciaDto>();
            var ids = new HashSet<string>(StringComparer.Ordinal);

            foreach (var file in files)
            {
                if (file == null || file.Length <= 0)
                {
                    continue;
                }

                await using var stream = file.OpenReadStream();
                var evidencia = await _service.UploadAsync(
                    stream,
                    file.FileName,
                    file.ContentType,
                    file.Length,
                    metadata,
                    cancellationToken);

                if (ids.Add(evidencia.Id))
                {
                    result.Add(evidencia);
                }
            }

            if (result.Count == 0)
            {
                throw new InvalidOperationException("No se recibieron archivos validos para procesar.");
            }

            return result;
        }
    }
}
