using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Inventario
{
    public class UbicacionDto
    {
        public long Id { get; set; }
        public long IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public string? Bloque { get; set; }
        public string? Piso { get; set; }
        public string? Sala { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string UbicacionCompleta => string.Join(" - ", 
            new[] { NombreSede, Bloque, Piso, Sala }.Where(x => !string.IsNullOrEmpty(x)));
    }

    public class UbicacionCreateDto
    {
        [Required(ErrorMessage = "La sede es requerida")]
        public long IdSede { get; set; }

        [StringLength(100)]
        public string? Bloque { get; set; }

        [StringLength(50)]
        public string? Piso { get; set; }

        [StringLength(150)]
        public string? Sala { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        public long IdUsuarioCreacion { get; set; }
    }

    public class UbicacionUpdateDto
    {
        [Required]
        public long Id { get; set; }

        public long? IdSede { get; set; }

        [StringLength(100)]
        public string? Bloque { get; set; }

        [StringLength(50)]
        public string? Piso { get; set; }

        [StringLength(150)]
        public string? Sala { get; set; }

        public string? Descripcion { get; set; }
    }
}
