using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public class DetalleEncuestaDto
    {
        public long Id { get; set; }
        public long IdEncuesta { get; set; }
        public long IdPregunta { get; set; }
        public string? TextoPregunta { get; set; }
        public long IdRespuesta { get; set; }
        public string? TextoRespuesta { get; set; }
        public int? ValorNumerico { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class DetalleEncuestaCreateDto
    {
        [Required(ErrorMessage = "El ID de la pregunta es requerido")]
        public long IdPregunta { get; set; }

        [Required(ErrorMessage = "El ID de la respuesta es requerido")]
        public long IdRespuesta { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }
}















