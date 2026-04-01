using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Catalogo
{
    [Table("TipoTrabajo", Schema = "catalogo")]
    public class TipoTrabajo
    {
        [Key]
        [Column("IdTipoTrabajo")]
        public long Id { get; set; }

        [Column("NombreTipoTrabajo")]
        [Required]
        [StringLength(100)]
        public string NombreTipoTrabajo { get; set; } = string.Empty;

        [Column("Descripcion")]
        public string? Descripcion { get; set; }

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















