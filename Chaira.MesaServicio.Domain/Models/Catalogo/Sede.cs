using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Catalogo
{
    [Table("Sede", Schema = "catalogo")]
    public class Sede
    {
        [Key]
        [Column("IdSede")]
        public long Id { get; set; }

        [Column("NombreSede")]
        [Required]
        [StringLength(150)]
        public string NombreSede { get; set; } = string.Empty;

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

        [ForeignKey("IdEstadoGeneral")]
        public virtual EstadoGeneral? EstadoGeneral { get; set; }
    }
}











