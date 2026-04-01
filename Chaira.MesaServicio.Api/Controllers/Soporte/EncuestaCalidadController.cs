using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chaira.MesaServicio.Infrastructure;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Soporte;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Soporte;
using Chaira.MesaServicio.Domain.Security;

namespace Chaira.MesaServicio.Api.Controllers.Soporte
{
    [ApiController]
    [Route("api/encuestas")]
    [Tags("EncuestaCalidad")]
    [Authorize(Roles = AppRoles.Todos)]
    public class EncuestaCalidadController : ControllerBase
    {
        private readonly IEncuestaCalidadService _service;
        private readonly ApplicationDbContext _context;

        public EncuestaCalidadController(
            IEncuestaCalidadService service,
            ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        /// <summary>
        /// Crear encuesta de calidad para un caso resuelto o cerrado
        /// </summary>
        [HttpPost("crear")]
        public async Task<ActionResult<ApiResponseDto<EncuestaCalidadResponseDto>>> Crear([FromBody] EncuestaCalidadCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponseDto<EncuestaCalidadResponseDto>.FailResponse("Datos invalidos"));
                }

                var caso = await _context.Casos
                    .AsNoTracking()
                    .Where(item => item.Id == dto.IdCaso)
                    .Select(item => new
                    {
                        item.Id,
                        item.NumeroCaso,
                        item.IdEstadoCaso,
                        item.IdUsuarioReporta,
                        item.IdAreaTecnica,
                        item.IdTecnicoAsignado
                    })
                    .FirstOrDefaultAsync();

                if (caso == null)
                {
                    return BadRequest(ApiResponseDto<EncuestaCalidadResponseDto>.FailResponse("Error al crear encuesta: El caso no existe."));
                }

                var nombreEstadoCaso = await _context.EstadosCaso
                    .AsNoTracking()
                    .Where(item => item.Id == caso.IdEstadoCaso)
                    .Select(item => item.NombreEstadoCaso)
                    .FirstOrDefaultAsync();

                if (!string.Equals(nombreEstadoCaso, "Resuelto", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(nombreEstadoCaso, "Cerrado", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(ApiResponseDto<EncuestaCalidadResponseDto>.FailResponse(
                        "Error al crear encuesta: Solo se puede crear encuesta para casos resueltos o cerrados."));
                }

                if (dto.IdUsuarioCreacion != caso.IdUsuarioReporta)
                {
                    return BadRequest(ApiResponseDto<EncuestaCalidadResponseDto>.FailResponse(
                        "Error al crear encuesta: Solo el usuario que reporto puede crear la encuesta."));
                }

                var existeEncuesta = await _context.EncuestasCalidad
                    .AsNoTracking()
                    .AnyAsync(item => item.IdCaso == dto.IdCaso);

                if (existeEncuesta)
                {
                    return BadRequest(ApiResponseDto<EncuestaCalidadResponseDto>.FailResponse(
                        "Error al crear encuesta: Ya existe una encuesta para este caso."));
                }

                await using var transaction = await _context.Database.BeginTransactionAsync();

                var fechaActual = DateTime.UtcNow;
                var encuestaEntity = new EncuestaCalidad
                {
                    IdCaso = dto.IdCaso,
                    FechaEncuesta = fechaActual,
                    Observaciones = dto.Observaciones,
                    IdUsuarioCreacion = dto.IdUsuarioCreacion
                };

                await _context.EncuestasCalidad.AddAsync(encuestaEntity);
                await _context.SaveChangesAsync();

                var detalles = dto.Detalles.Select(item => new DetalleEncuesta
                {
                    IdEncuesta = encuestaEntity.Id,
                    IdPregunta = item.IdPregunta,
                    IdRespuesta = item.IdRespuesta,
                    FechaRegistro = fechaActual,
                    IdUsuarioCreacion = item.IdUsuarioCreacion
                }).ToList();

                if (detalles.Count > 0)
                {
                    await _context.DetallesEncuesta.AddRangeAsync(detalles);
                }

                await _context.TrazabilidadesCaso.AddAsync(new TrazabilidadCaso
                {
                    IdCaso = caso.Id,
                    FechaEvento = fechaActual,
                    IdUsuarioAccion = dto.IdUsuarioCreacion,
                    TipoEvento = "EncuestaCreada",
                    Comentario = $"Encuesta de calidad completada. {dto.Observaciones}".Trim(),
                    IdEstadoCaso = caso.IdEstadoCaso,
                    IdAreaTecnica = caso.IdAreaTecnica,
                    IdTecnicoAsignado = caso.IdTecnicoAsignado
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var encuestaCreada = await _service.GetByIdAsync(encuestaEntity.Id);
                if (encuestaCreada == null)
                {
                    return BadRequest(ApiResponseDto<EncuestaCalidadResponseDto>.FailResponse(
                        "Error al crear encuesta: No se pudo recuperar la encuesta creada."));
                }

                var encuesta = await BuildResponseAsync(encuestaCreada, caso.NumeroCaso);

                return CreatedAtAction(nameof(Crear), new { id = encuesta.Id },
                    ApiResponseDto<EncuestaCalidadResponseDto>.SuccessResponse(encuesta, "Encuesta de calidad creada exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<EncuestaCalidadResponseDto>.FailResponse($"Error al crear encuesta: {ex.Message}"));
            }
        }

        [HttpGet("caso/{idCaso:long}")]
        public async Task<ActionResult<ApiResponseDto<EncuestaCalidadDto>>> ObtenerPorCaso(long idCaso)
        {
            var encuesta = (await _service.GetByCasoIdAsync(idCaso))
                .OrderByDescending(item => item.FechaEncuesta)
                .FirstOrDefault();

            if (encuesta == null)
            {
                return NotFound(ApiResponseDto<EncuestaCalidadDto>.FailResponse("No existe encuesta para este caso."));
            }

            return Ok(ApiResponseDto<EncuestaCalidadDto>.SuccessResponse(encuesta, "Encuesta obtenida"));
        }

        [HttpGet("catalogo")]
        public async Task<ActionResult<ApiResponseDto<EncuestaCalidadCatalogoDto>>> ObtenerCatalogo()
        {
            var estadoActivoId = await _context.EstadosGenerales
                .AsNoTracking()
                .Where(item => item.NombreEstado == "Activo")
                .Select(item => (long?)item.Id)
                .FirstOrDefaultAsync();

            var preguntasQuery = _context.Preguntas.AsNoTracking();
            var respuestasQuery = _context.Respuestas.AsNoTracking();

            if (estadoActivoId.HasValue)
            {
                preguntasQuery = preguntasQuery.Where(item => item.IdEstadoGeneral == estadoActivoId.Value);
                respuestasQuery = respuestasQuery.Where(item => item.IdEstadoGeneral == estadoActivoId.Value);
            }

            var preguntas = await preguntasQuery
                .OrderBy(item => item.Id)
                .Select(item => new PreguntaEncuestaCatalogoDto
                {
                    Id = item.Id,
                    TextoPregunta = item.TextoPregunta
                })
                .ToListAsync();

            var respuestas = await respuestasQuery
                .OrderByDescending(item => item.ValorNumerico)
                .ThenBy(item => item.Id)
                .Select(item => new RespuestaEncuestaCatalogoDto
                {
                    Id = item.Id,
                    TextoRespuesta = item.TextoRespuesta,
                    ValorNumerico = item.ValorNumerico
                })
                .ToListAsync();

            var data = new EncuestaCalidadCatalogoDto
            {
                Preguntas = preguntas,
                Respuestas = respuestas
            };

            return Ok(ApiResponseDto<EncuestaCalidadCatalogoDto>.SuccessResponse(data, "Catalogo de encuesta obtenido"));
        }

        private async Task<EncuestaCalidadResponseDto> BuildResponseAsync(EncuestaCalidadDto creada, string? numeroCaso = null)
        {
            var numeroCasoReal = numeroCaso;
            if (string.IsNullOrWhiteSpace(numeroCasoReal))
            {
                numeroCasoReal = await _context.Casos
                    .AsNoTracking()
                    .Where(item => item.Id == creada.IdCaso)
                    .Select(item => item.NumeroCaso)
                    .FirstOrDefaultAsync();
            }

            var nombreUsuario = await _context.Usuarios
                .AsNoTracking()
                .Where(item => item.Id == creada.IdUsuarioCreacion)
                .Select(item => item.NombreCompleto)
                .FirstOrDefaultAsync();

            return new EncuestaCalidadResponseDto
            {
                Id = creada.Id,
                IdCaso = creada.IdCaso,
                NumeroCaso = numeroCasoReal ?? string.Empty,
                FechaEncuesta = creada.FechaEncuesta,
                Observaciones = creada.Observaciones,
                IdUsuarioCreacion = creada.IdUsuarioCreacion,
                NombreUsuario = nombreUsuario ?? string.Empty,
                CantidadRespuestas = creada.Detalles?.Count ?? 0,
                Respuestas = creada.Detalles?.Select(item => new DetalleRespuestaDto
                {
                    Id = item.Id,
                    IdPregunta = item.IdPregunta,
                    TextoPregunta = item.TextoPregunta ?? string.Empty,
                    IdRespuesta = item.IdRespuesta,
                    TextoRespuesta = item.TextoRespuesta ?? string.Empty,
                    ValorNumerico = item.ValorNumerico,
                    FechaRegistro = item.FechaRegistro
                }).ToList()
            };
        }
    }
}

