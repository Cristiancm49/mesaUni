using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Models.Inventario
{
    [Table("Ubicacion", Schema = "inventario")]
    public class Ubicacion
    {
        [Key]
        [Column("IdUbicacion")]
        public long Id { get; set; }

        [Column("IdSede")]
        [Required]
        public long IdSede { get; set; }

        [Column("Bloque")]
        [StringLength(100)]
        public string? Bloque { get; set; }

        [Column("Piso")]
        [StringLength(50)]
        public string? Piso { get; set; }

        [Column("Sala")]
        [StringLength(150)]
        public string? Sala { get; set; }

        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [Column("FechaCreacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        [ForeignKey("IdSede")]
        public virtual Sede? Sede { get; set; }
    }
}
