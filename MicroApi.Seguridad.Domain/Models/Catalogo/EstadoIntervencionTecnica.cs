using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroApi.Seguridad.Domain.Models.Catalogo
{
    [Table("EstadoIntervencionTecnica", Schema = "catalogo")]
    public class EstadoIntervencionTecnica
    {
        [Key]
        [Column("IdEstadoIntervencion")]
        public long Id { get; set; }

        [Column("NombreEstado")]
        [Required]
        [StringLength(100)]
        public string NombreEstado { get; set; } = string.Empty;

        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [Column("FechaCreacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        [Column("IdEstadoGeneral")]
        [Required]
        public long IdEstadoGeneral { get; set; }

        [ForeignKey("IdEstadoGeneral")]
        public virtual EstadoGeneral EstadoGeneral { get; set; } = null!;
    }
}
