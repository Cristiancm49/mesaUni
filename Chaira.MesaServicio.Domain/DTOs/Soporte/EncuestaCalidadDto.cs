using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public class EncuestaCalidadDto
    {
        public long Id { get; set; }
        public long IdCaso { get; set; }
        public DateTime FechaEncuesta { get; set; }
        public string? Observaciones { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public List<DetalleEncuestaDto>? Detalles { get; set; }
    }

    public class EncuestaCalidadCreateDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public long IdUsuarioCreacion { get; set; }

        [Required(ErrorMessage = "Las respuestas son requeridas")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos una respuesta")]
        public List<RespuestaEncuestaDto> Respuestas { get; set; } = new();

        [Required(ErrorMessage = "Los detalles son requeridos")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un detalle")]
        public List<DetalleEncuestaCreateDto> Detalles { get; set; } = new();
    }

    public class RespuestaEncuestaDto
    {
        [Required]
        public long IdPregunta { get; set; }

        [Required]
        public long IdRespuesta { get; set; }

        [Range(1, 10, ErrorMessage = "El valor numÃ©rico debe estar entre 1 y 10")]
        public int? ValorNumerico { get; set; }
    }

    public class EncuestaCalidadResponseDto
    {
        public long Id { get; set; }
        public long IdCaso { get; set; }
        public string NumeroCaso { get; set; } = string.Empty;
        public DateTime FechaEncuesta { get; set; }
        public string? Observaciones { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int CantidadRespuestas { get; set; }
        public List<DetalleRespuestaDto>? Respuestas { get; set; }
    }

    public class DetalleRespuestaDto
    {
        public long Id { get; set; }
        public long IdPregunta { get; set; }
        public string TextoPregunta { get; set; } = string.Empty;
        public long IdRespuesta { get; set; }
        public string TextoRespuesta { get; set; } = string.Empty;
        public int? ValorNumerico { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class EncuestaCalidadCatalogoDto
    {
        public List<PreguntaEncuestaCatalogoDto> Preguntas { get; set; } = new();
        public List<RespuestaEncuestaCatalogoDto> Respuestas { get; set; } = new();
    }

    public class PreguntaEncuestaCatalogoDto
    {
        public long Id { get; set; }
        public string TextoPregunta { get; set; } = string.Empty;
    }

    public class RespuestaEncuestaCatalogoDto
    {
        public long Id { get; set; }
        public string TextoRespuesta { get; set; } = string.Empty;
        public int? ValorNumerico { get; set; }
    }
}

