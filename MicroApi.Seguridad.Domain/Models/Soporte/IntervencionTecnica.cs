using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Models.Soporte
{
    [Table("IntervencionTecnica", Schema = "soporte")]
    public class IntervencionTecnica
    {
        [Key]
        [Column("IdIntervencionTecnica")]
        public long Id { get; set; }

        [Column("IdTrazabilidadCaso")]
        [Required]
        public long IdTrazabilidadCaso { get; set; }

        [Column("IdTipoTrabajo")]
        [Required]
        public long IdTipoTrabajo { get; set; }

        [Column("IdEstadoIntervencion")]
        [Required]
        public long IdEstadoIntervencion { get; set; }

        [Column("FechaInicio")]
        [Required]
        public DateTime FechaInicio { get; set; }

        [Column("FechaFin")]
        public DateTime? FechaFin { get; set; }

        [Column("Diagnostico")]
        public string? Diagnostico { get; set; }

        [Column("SolucionAplicada")]
        public string? SolucionAplicada { get; set; }

        [Column("IdUsuarioAccion")]
        [Required]
        public long IdUsuarioAccion { get; set; }

        // Navegación
        [ForeignKey("IdTrazabilidadCaso")]
        public virtual TrazabilidadCaso TrazabilidadCaso { get; set; } = null!;

        [ForeignKey("IdTipoTrabajo")]
        public virtual TipoTrabajo TipoTrabajo { get; set; } = null!;

        [ForeignKey("IdEstadoIntervencion")]
        public virtual EstadoIntervencionTecnica EstadoIntervencion { get; set; } = null!;

        public virtual ICollection<DetalleCambioComponentes> CambiosComponentes { get; set; } = new List<DetalleCambioComponentes>();
        public virtual ICollection<DetalleConsumible> DetallesConsumibles { get; set; } = new List<DetalleConsumible>();
        public virtual ICollection<RevisionAdmi> RevisionesAdmi { get; set; } = new List<RevisionAdmi>();
    }
}














