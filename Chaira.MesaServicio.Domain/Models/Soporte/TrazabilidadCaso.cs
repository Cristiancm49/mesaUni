using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Models.Soporte
{
    [Table("TrazabilidadCaso", Schema = "soporte")]
    public class TrazabilidadCaso
    {
        [Key]
        [Column("IdTrazabilidadCaso")]
        public long Id { get; set; }

        [Column("IdCaso")]
        [Required]
        public long IdCaso { get; set; }

        [Column("FechaEvento")]
        [Required]
        public DateTime FechaEvento { get; set; }

        [Column("IdUsuarioAccion")]
        [Required]
        public long IdUsuarioAccion { get; set; }

        [Column("TipoEvento")]
        [Required]
        [StringLength(100)]
        public string TipoEvento { get; set; } = string.Empty; // Creacion, CambioEstado, Asignacion, Reasignacion, Comentario, Resolucion, Cierre

        [Column("Comentario")]
        public string? Comentario { get; set; }

        [Column("IdEstadoCaso")]
        public long? IdEstadoCaso { get; set; }

        [Column("IdAreaTecnica")]
        public long? IdAreaTecnica { get; set; }

        [Column("IdTecnicoAsignado")]
        public long? IdTecnicoAsignado { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdCaso")]
        public virtual Caso Caso { get; set; } = null!;

        [ForeignKey("IdEstadoCaso")]
        public virtual EstadoCaso? EstadoCaso { get; set; }

        [ForeignKey("IdAreaTecnica")]
        public virtual AreaTecnica? AreaTecnica { get; set; }

        public virtual ICollection<IntervencionTecnica> IntervencionesTecnicas { get; set; } = new List<IntervencionTecnica>();
    }
}















