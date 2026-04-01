using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Models.Inventario
{
    [Table("Consumible", Schema = "inventario")]
    public class Consumible
    {
        [Key]
        [Column("IdConsumible")]
        public long Id { get; set; }

        [Column("NombreConsumible")]
        [Required]
        [StringLength(200)]
        public string NombreConsumible { get; set; } = string.Empty;

        [Column("Marca")]
        [StringLength(100)]
        public string? Marca { get; set; }

        [Column("Modelo")]
        [StringLength(100)]
        public string? Modelo { get; set; }

        [Column("StockActual")]
        [Required]
        public int StockActual { get; set; }

        [Column("StockMinimo")]
        [Required]
        public int StockMinimo { get; set; }

        [Column("DescripcionTecnica")]
        public string? DescripcionTecnica { get; set; }

        [Column("IdInventario")]
        [Required]
        public long IdInventario { get; set; }

        [Column("IdTipoConsumible")]
        [Required]
        public long IdTipoConsumible { get; set; }

        [Column("IdEstadoConsumible")]
        [Required]
        public long IdEstadoConsumible { get; set; }

        [Column("FechaCreacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("FechaActualizacion")]
        public DateTime? FechaActualizacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdInventario")]
        public virtual Inventario Inventario { get; set; } = null!;

        [ForeignKey("IdTipoConsumible")]
        public virtual TipoConsumible TipoConsumible { get; set; } = null!;

        [ForeignKey("IdEstadoConsumible")]
        public virtual EstadoConsumible EstadoConsumible { get; set; } = null!;
    }
}















