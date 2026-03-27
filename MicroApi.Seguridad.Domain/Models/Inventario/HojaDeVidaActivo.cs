using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroApi.Seguridad.Domain.Models.Acceso;

namespace MicroApi.Seguridad.Domain.Models.Inventario
{
    [Table("HojaDeVidaActivo", Schema = "inventario")]
    public class HojaDeVidaActivo
    {
        [Key]
        [Column("IdHojaActivo")]
        public long Id { get; set; }

        [Column("IdActivo")]
        [Required]
        public long IdActivo { get; set; }

        [Column("FechaRegistro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("DetalleRegistro")]
        [Required]
        public string DetalleRegistro { get; set; } = string.Empty;

        [Column("TipoEvento")]
        [StringLength(100)]
        public string? TipoEvento { get; set; }

        [Column("IdCaso")]
        public long? IdCaso { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // Navegación
        [ForeignKey("IdActivo")]
        public virtual Activo Activo { get; set; } = null!;

        [ForeignKey("IdUsuarioCreacion")]
        public virtual Usuario UsuarioCreacion { get; set; } = null!;
    }
}



