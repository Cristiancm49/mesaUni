using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chaira.MesaServicio.Domain.Models.Acceso
{
    [Table("Usuario", Schema = "acceso")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        public long Id { get; set; }

        [Column("NombreCompleto")]
        [Required]
        [StringLength(150)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("Email")]
        [Required]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Column("Telefono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Column("IdRol")]
        [Required]
        public long IdRol { get; set; }

        [Column("FechaCreacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioCreacion")]
        [Required]
        public long IdUsuarioCreacion { get; set; }

        // NavegaciÃ³n
        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; } = null!;
    }
}















