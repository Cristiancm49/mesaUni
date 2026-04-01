using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Catalogo
{
    [Table("Pregunta", Schema = "catalogo")]
    public class Pregunta
    {
        [Key]
        [Column("IdPregunta")]
        public long Id { get; set; }

        [Column("TextoPregunta")]
        [Required]
        [StringLength(100)]
        public string TextoPregunta { get; set; } = string.Empty;

        [Column("IdEstadoGeneral")]
        [Required]
        public long IdEstadoGeneral { get; set; }

        [Column("FechaCreacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdEstadoGeneral")]
        public virtual EstadoGeneral EstadoGeneral { get; set; } = null!;
    }
}















