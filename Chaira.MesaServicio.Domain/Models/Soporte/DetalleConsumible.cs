using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Models.Soporte
{
    [Table("DetalleConsumible", Schema = "soporte")]
    public class DetalleConsumible
    {
        [Key]
        [Column("IdDetalleConsumible")]
        public long Id { get; set; }

        [Column("IdIntervencionTecnica")]
        [Required]
        public long IdIntervencionTecnica { get; set; }

        [Column("IdConsumible")]
        [Required]
        public long IdConsumible { get; set; }

        [Column("Cantidad")]
        [Required]
        public int Cantidad { get; set; }

        [Column("DescripcionUso")]
        public string? DescripcionUso { get; set; }

        [Column("FechaRegistro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdIntervencionTecnica")]
        public virtual IntervencionTecnica IntervencionTecnica { get; set; } = null!;

        [ForeignKey("IdConsumible")]
        public virtual Consumible Consumible { get; set; } = null!;
    }
}















