using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Soporte
{
    [Table("EncuestaCalidad", Schema = "soporte")]
    public class EncuestaCalidad
    {
        [Key]
        [Column("IdEncuesta")]
        public long Id { get; set; }

        [Column("IdCaso")]
        [Required]
        public long IdCaso { get; set; }

        [Column("FechaEncuesta")]
        [Required]
        public DateTime FechaEncuesta { get; set; }

        [Column("Observaciones")]
        public string? Observaciones { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdCaso")]
        public virtual Caso Caso { get; set; } = null!;

        public virtual ICollection<DetalleEncuesta> Detalles { get; set; } = new List<DetalleEncuesta>();
    }
}















