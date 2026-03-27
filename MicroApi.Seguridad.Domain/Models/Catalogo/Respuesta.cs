using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroApi.Seguridad.Domain.Models.Catalogo
{
    [Table("Respuesta", Schema = "catalogo")]
    public class Respuesta
    {
        [Key]
        [Column("IdRespuesta")]
        public long Id { get; set; }

        [Column("TextoRespuesta")]
        [Required]
        [StringLength(50)]
        public string TextoRespuesta { get; set; } = string.Empty;

        [Column("ValorNumerico")]
        public int? ValorNumerico { get; set; }

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
    }
}














