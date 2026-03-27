using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Models.Soporte
{
    [Table("DetalleCambioComponentes", Schema = "soporte")]
    public class DetalleCambioComponentes
    {
        [Key]
        [Column("IdCambioComponente")]
        public long Id { get; set; }

        [Column("IdIntervencionTecnica")]
        [Required]
        public long IdIntervencionTecnica { get; set; }

        [Column("IdComponente")]
        [Required]
        public long IdComponente { get; set; }

        [Column("Cantidad")]
        [Required]
        public int Cantidad { get; set; }

        [Column("TipoCambio")]
        [Required]
        [StringLength(20)]
        public string TipoCambio { get; set; } = string.Empty; // INSTALACION, RETIRO, REEMPLAZO

        [Column("DescripcionCambio")]
        public string? DescripcionCambio { get; set; }

        [Column("FechaRegistro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // Navegación
        [ForeignKey("IdIntervencionTecnica")]
        public virtual IntervencionTecnica IntervencionTecnica { get; set; } = null!;

        [ForeignKey("IdComponente")]
        public virtual Componente Componente { get; set; } = null!;
    }
}














