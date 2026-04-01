using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Catalogo
{
    [Table("Prioridad", Schema = "catalogo")]
    public class Prioridad
    {
        [Key]
        [Column("IdPrioridad")]
        public long Id { get; set; }

        [Column("NombrePrioridad")]
        [Required]
        [StringLength(100)]
        public string NombrePrioridad { get; set; } = string.Empty;

        [Column("TiempoRespuestaDias")]
        [Required]
        public int TiempoRespuestaDias { get; set; }

        [Column("TiempoResolucionDias")]
        [Required]
        public int TiempoResolucionDias { get; set; }

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















