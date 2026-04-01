using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Models.Soporte
{
    [Table("DetalleEncuesta", Schema = "soporte")]
    public class DetalleEncuesta
    {
        [Key]
        [Column("IdDetalleEncuesta")]
        public long Id { get; set; }

        [Column("IdEncuesta")]
        [Required]
        public long IdEncuesta { get; set; }

        [Column("IdPregunta")]
        [Required]
        public long IdPregunta { get; set; }

        [Column("IdRespuesta")]
        [Required]
        public long IdRespuesta { get; set; }

        [Column("FechaRegistro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdEncuesta")]
        public virtual EncuestaCalidad Encuesta { get; set; } = null!;

        [ForeignKey("IdPregunta")]
        public virtual Pregunta Pregunta { get; set; } = null!;

        [ForeignKey("IdRespuesta")]
        public virtual Respuesta Respuesta { get; set; } = null!;
    }
}















