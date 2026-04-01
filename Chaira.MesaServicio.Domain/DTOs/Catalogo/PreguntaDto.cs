using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Catalogo
{
    public class PreguntaDto
    {
        public long Id { get; set; }
        public string TextoPregunta { get; set; } = string.Empty;
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class PreguntaCreateDto
    {
        [Required(ErrorMessage = "El texto de la pregunta es requerido")]
        [StringLength(100, ErrorMessage = "El texto no puede exceder 100 caracteres")]
        public string TextoPregunta { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class PreguntaUpdateDto
    {
        [Required(ErrorMessage = "El ID de la pregunta es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El texto no puede exceder 100 caracteres")]
        public string? TextoPregunta { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}















