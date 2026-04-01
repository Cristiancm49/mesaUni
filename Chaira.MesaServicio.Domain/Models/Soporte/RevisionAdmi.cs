using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Soporte
{
    [Table("RevisionAdmi", Schema = "soporte")]
    public class RevisionAdmi
    {
        [Key]
        [Column("IdRevisionAdmi")]
        public long Id { get; set; }

        [Column("IdIntervencionTecnica")]
        [Required]
        public long IdIntervencionTecnica { get; set; }

        [Column("Aprobado")]
        [Required]
        public bool Aprobado { get; set; }

        [Column("ObservacionRevision")]
        public string? ObservacionRevision { get; set; }

        [Column("TipoRevision")]
        [Required]
        public string TipoRevision { get; set; } = "DIAGNOSTICO";

        [Column("FechaRegistro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdIntervencionTecnica")]
        public virtual IntervencionTecnica IntervencionTecnica { get; set; } = null!;
    }
}















