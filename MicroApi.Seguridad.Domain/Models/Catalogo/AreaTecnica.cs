using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroApi.Seguridad.Domain.Models.Acceso;

namespace MicroApi.Seguridad.Domain.Models.Catalogo
{
    [Table("AreaTecnica", Schema = "catalogo")]
    public class AreaTecnica
    {
        [Key]
        [Column("IdAreaTecnica")]
        public long Id { get; set; }

        [Column("NombreAreaTecnica")]
        [Required]
        [StringLength(100)]
        public string NombreAreaTecnica { get; set; } = string.Empty;

        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [Column("IdEncargado")]
        [Required]
        public long IdEncargado { get; set; }

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
        [ForeignKey("IdEstadoGeneral")]
        public virtual EstadoGeneral EstadoGeneral { get; set; } = null!;

        [ForeignKey("IdEncargado")]
        public virtual Usuario Encargado { get; set; } = null!;
    }
}












