using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Catalogo
{
    public class RespuestaDto
    {
        public long Id { get; set; }
        public string TextoRespuesta { get; set; } = string.Empty;
        public int? ValorNumerico { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class RespuestaCreateDto
    {
        [Required(ErrorMessage = "El texto de la respuesta es requerido")]
        [StringLength(50, ErrorMessage = "El texto no puede exceder 50 caracteres")]
        public string TextoRespuesta { get; set; } = string.Empty;

        public int? ValorNumerico { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class RespuestaUpdateDto
    {
        [Required(ErrorMessage = "El ID de la respuesta es requerido")]
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "El texto no puede exceder 50 caracteres")]
        public string? TextoRespuesta { get; set; }

        public int? ValorNumerico { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}














