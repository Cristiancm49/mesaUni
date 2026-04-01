using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chaira.MesaServicio.Domain.Models.Acceso;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Domain.Models.Inventario
{
    [Table("Inventario", Schema = "inventario")]
    public class Inventario
    {
        [Key]
        [Column("IdInventario")]
        public long Id { get; set; }

        [Column("NombreInventario")]
        [Required]
        [StringLength(200)]
        public string NombreInventario { get; set; } = string.Empty;

        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [Column("IdEstadoGeneral")]
        [Required]
        public long IdEstadoGeneral { get; set; }

        [Column("IdResponsableInventario")]
        [Required]
        public long IdResponsableInventario { get; set; }

        [Column("FechaCreacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdEstadoGeneral")]
        public virtual EstadoGeneral EstadoGeneral { get; set; } = null!;

        [ForeignKey("IdResponsableInventario")]
        public virtual Usuario Responsable { get; set; } = null!;

        public virtual ICollection<Activo> Activos { get; set; } = new List<Activo>();
        public virtual ICollection<Componente> Componentes { get; set; } = new List<Componente>();
        public virtual ICollection<Consumible> Consumibles { get; set; } = new List<Consumible>();
    }
}




