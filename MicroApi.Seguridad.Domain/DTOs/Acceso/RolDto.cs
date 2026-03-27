using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Acceso
{
    public class RolDto
    {
        public long Id { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstadoGeneral { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class RolCreateDto
    {
        [Required(ErrorMessage = "El nombre del rol es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreRol { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creacion es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class RolUpdateDto
    {
        [Required(ErrorMessage = "El ID del rol es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombreRol { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}
