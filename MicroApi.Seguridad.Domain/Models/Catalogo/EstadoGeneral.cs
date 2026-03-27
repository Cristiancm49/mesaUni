using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroApi.Seguridad.Domain.Models.Catalogo
{
    [Table("EstadoGeneral", Schema = "catalogo")]
    public class EstadoGeneral
    {
        [Key]
        [Column("IdEstadoGeneral")]
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

        [Column("Activo")]
        [Required]
        public bool Activo { get; set; } = true;
    }
}














