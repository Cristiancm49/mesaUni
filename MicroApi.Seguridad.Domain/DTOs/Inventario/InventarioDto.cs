using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Inventario
{
    public class InventarioDto
    {
        public long Id { get; set; }
        public string NombreInventario { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public long IdResponsableInventario { get; set; }
        public string? NombreResponsable { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class InventarioCreateDto
    {
        [Required(ErrorMessage = "El nombre del inventario es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string NombreInventario { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del responsable es requerido")]
        public long IdResponsableInventario { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class InventarioUpdateDto
    {
        [Required(ErrorMessage = "El ID del inventario es requerido")]
        public long Id { get; set; }

        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string? NombreInventario { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEstadoGeneral { get; set; }

        public long? IdResponsableInventario { get; set; }
    }
}














