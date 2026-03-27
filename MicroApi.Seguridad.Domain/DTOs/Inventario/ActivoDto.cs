using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Inventario
{
    public class ActivoDto
    {
        public long Id { get; set; }
        public string? CodigoPatrimonial { get; set; }
        public string NombreActivo { get; set; } = string.Empty;
        public string? DescripcionTecnica { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Serie { get; set; }
        public long IdCategoriaActivo { get; set; }
        public string? NombreCategoria { get; set; }
        public long IdEstadoActivo { get; set; }
        public string? NombreEstadoActivo { get; set; }
        public long IdInventario { get; set; }
        public string? NombreInventario { get; set; }
        public long IdUbicacion { get; set; }
        public string? UbicacionCompleta { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public long? IdResponsableActivo { get; set; }
        public string? NombreResponsable { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class ActivoCreateDto
    {
        [StringLength(100, ErrorMessage = "El código patrimonial no puede exceder 100 caracteres")]
        public string? CodigoPatrimonial { get; set; }

        [Required(ErrorMessage = "El nombre del activo es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string NombreActivo { get; set; } = string.Empty;

        public string? DescripcionTecnica { get; set; }

        [StringLength(100, ErrorMessage = "La marca no puede exceder 100 caracteres")]
        public string? Marca { get; set; }

        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres")]
        public string? Modelo { get; set; }

        [StringLength(100, ErrorMessage = "La serie no puede exceder 100 caracteres")]
        public string? Serie { get; set; }

        [Required(ErrorMessage = "El ID de la categoría es requerido")]
        public long IdCategoriaActivo { get; set; }

        [Required(ErrorMessage = "El ID del estado del activo es requerido")]
        public long IdEstadoActivo { get; set; }

        [Required(ErrorMessage = "El ID del inventario es requerido")]
        public long IdInventario { get; set; }

        [Required(ErrorMessage = "El ID de la ubicación es requerido")]
        public long IdUbicacion { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public long? IdResponsableActivo { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class ActivoUpdateDto
    {
        [Required(ErrorMessage = "El ID del activo es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El código patrimonial no puede exceder 100 caracteres")]
        public string? CodigoPatrimonial { get; set; }

        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string? NombreActivo { get; set; }

        public string? DescripcionTecnica { get; set; }

        [StringLength(100, ErrorMessage = "La marca no puede exceder 100 caracteres")]
        public string? Marca { get; set; }

        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres")]
        public string? Modelo { get; set; }

        [StringLength(100, ErrorMessage = "La serie no puede exceder 100 caracteres")]
        public string? Serie { get; set; }

        public long? IdCategoriaActivo { get; set; }

        public long? IdEstadoActivo { get; set; }

        public long? IdInventario { get; set; }

        public long? IdUbicacion { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public long? IdResponsableActivo { get; set; }
    }

    public class ActivoFiltrosDto
    {
        public long? IdInventario { get; set; }
        public long? IdCategoriaActivo { get; set; }
        public long? IdEstadoActivo { get; set; }
        public long? IdUbicacion { get; set; }
        public string? Busqueda { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}














