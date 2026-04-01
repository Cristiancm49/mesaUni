using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chaira.MesaServicio.Domain.Models.Acceso;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Models.Inventario
{
    [Table("Activo", Schema = "inventario")]
    public class Activo
    {
        [Key]
        [Column("IdActivo")]
        public long Id { get; set; }

        [Column("CodigoPatrimonial")]
        [StringLength(100)]
        public string? CodigoPatrimonial { get; set; }

        [Column("NombreActivo")]
        [Required]
        [StringLength(200)]
        public string NombreActivo { get; set; } = string.Empty;

        [Column("DescripcionTecnica")]
        public string? DescripcionTecnica { get; set; }

        [Column("Marca")]
        [StringLength(100)]
        public string? Marca { get; set; }

        [Column("Modelo")]
        [StringLength(100)]
        public string? Modelo { get; set; }

        [Column("Serie")]
        [StringLength(100)]
        public string? Serie { get; set; }

        [Column("IdCategoriaActivo")]
        [Required]
        public long IdCategoriaActivo { get; set; }

        [Column("IdEstadoActivo")]
        [Required]
        public long IdEstadoActivo { get; set; }

        [Column("IdInventario")]
        [Required]
        public long IdInventario { get; set; }

        [Column("IdUbicacion")]
        [Required]
        public long IdUbicacion { get; set; }

        [Column("FechaIngreso")]
        public DateTime? FechaIngreso { get; set; }

        [Column("IdResponsableActivo")]
        public long? IdResponsableActivo { get; set; }

        [Column("FechaCreacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("FechaActualizacion")]
        public DateTime? FechaActualizacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdCategoriaActivo")]
        public virtual CategoriaActivo CategoriaActivo { get; set; } = null!;

        [ForeignKey("IdEstadoActivo")]
        public virtual EstadoActivo EstadoActivo { get; set; } = null!;

        [ForeignKey("IdInventario")]
        public virtual Inventario Inventario { get; set; } = null!;

        [ForeignKey("IdUbicacion")]
        public virtual Ubicacion Ubicacion { get; set; } = null!;

        [ForeignKey("IdResponsableActivo")]
        public virtual Usuario? Responsable { get; set; }

        public virtual ICollection<HojaDeVidaActivo> HojasDeVida { get; set; } = new List<HojaDeVidaActivo>();
    }
}




