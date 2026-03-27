using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Models.Inventario
{
    [Table("Componente", Schema = "inventario")]
    public class Componente
    {
        [Key]
        [Column("IdComponente")]
        public long Id { get; set; }

        [Column("NombreComponente")]
        [Required]
        [StringLength(200)]
        public string NombreComponente { get; set; } = string.Empty;

        [Column("Marca")]
        [StringLength(100)]
        public string? Marca { get; set; }

        [Column("Modelo")]
        [StringLength(100)]
        public string? Modelo { get; set; }

        [Column("IdInventario")]
        [Required]
        public long IdInventario { get; set; }

        [Column("StockActual")]
        [Required]
        public int StockActual { get; set; }

        [Column("StockMinimo")]
        [Required]
        public int StockMinimo { get; set; }

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

        // Navegación
        [ForeignKey("IdInventario")]
        public virtual Inventario Inventario { get; set; } = null!;

        [ForeignKey("IdEstadoGeneral")]
        public virtual EstadoGeneral EstadoGeneral { get; set; } = null!;
    }
}














