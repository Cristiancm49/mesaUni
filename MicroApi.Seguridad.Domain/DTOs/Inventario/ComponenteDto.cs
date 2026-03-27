using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Inventario
{
    public class ComponenteDto
    {
        public long Id { get; set; }
        public string NombreComponente { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public long IdInventario { get; set; }
        public string? NombreInventario { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public string? Descripcion { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }

        /// <summary>
        /// Indica si el stock actual está por debajo del mínimo
        /// </summary>
        public bool StockBajo => StockActual < StockMinimo;
    }

    public class ComponenteCreateDto
    {
        [Required(ErrorMessage = "El nombre del componente es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string NombreComponente { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "La marca no puede exceder 100 caracteres")]
        public string? Marca { get; set; }

        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres")]
        public string? Modelo { get; set; }

        [Required(ErrorMessage = "El ID del inventario es requerido")]
        public long IdInventario { get; set; }

        [Required(ErrorMessage = "El stock actual es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock actual debe ser mayor o igual a 0")]
        public int StockActual { get; set; }

        [Required(ErrorMessage = "El stock mínimo es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
        public int StockMinimo { get; set; }

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class ComponenteUpdateDto
    {
        [Required(ErrorMessage = "El ID del componente es requerido")]
        public long Id { get; set; }

        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string? NombreComponente { get; set; }

        [StringLength(100, ErrorMessage = "La marca no puede exceder 100 caracteres")]
        public string? Marca { get; set; }

        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres")]
        public string? Modelo { get; set; }

        public long? IdInventario { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock actual debe ser mayor o igual a 0")]
        public int? StockActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
        public int? StockMinimo { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}














