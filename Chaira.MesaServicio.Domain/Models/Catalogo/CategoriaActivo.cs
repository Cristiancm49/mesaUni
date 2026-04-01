using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Catalogo
{
    [Table("CategoriaActivo", Schema = "catalogo")]
    public class CategoriaActivo
    {
        [Key]
        [Column("IdCategoriaActivo")]
        public long Id { get; set; }

        [Column("NombreCategoria")]
        [Required]
        [StringLength(100)]
        public string NombreCategoria { get; set; } = string.Empty;

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

        // NavegaciÃ³n
        [ForeignKey("IdEstadoGeneral")]
        public virtual EstadoGeneral EstadoGeneral { get; set; } = null!;
    }
}















