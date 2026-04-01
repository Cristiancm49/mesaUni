using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Inventario
{
    public class ConsumibleDto
    {
        public long Id { get; set; }
        public string NombreConsumible { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public string? DescripcionTecnica { get; set; }
        public long IdInventario { get; set; }
        public string? NombreInventario { get; set; }
        public long IdTipoConsumible { get; set; }
        public string? NombreTipo { get; set; }
        public long IdEstadoConsumible { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        /// <summary>
        /// Indica si el stock actual estÃ¡ por debajo del mÃ­nimo
        /// </summary>
        public bool StockBajo => StockActual < StockMinimo;
    }

    public class ConsumibleCreateDto
    {
        [Required(ErrorMessage = "El nombre del consumible es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string NombreConsumible { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "La marca no puede exceder 100 caracteres")]
        public string? Marca { get; set; }

        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres")]
        public string? Modelo { get; set; }

        [Required(ErrorMessage = "El stock actual es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock actual debe ser mayor o igual a 0")]
        public int StockActual { get; set; }

        [Required(ErrorMessage = "El stock mÃ­nimo es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock mÃ­nimo debe ser mayor o igual a 0")]
        public int StockMinimo { get; set; }

        public string? DescripcionTecnica { get; set; }

        [Required(ErrorMessage = "El ID del inventario es requerido")]
        public long IdInventario { get; set; }

        [Required(ErrorMessage = "El ID del tipo de consumible es requerido")]
        public long IdTipoConsumible { get; set; }

        [Required(ErrorMessage = "El ID del estado del consumible es requerido")]
        public long IdEstadoConsumible { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class ConsumibleUpdateDto
    {
        [Required(ErrorMessage = "El ID del consumible es requerido")]
        public long Id { get; set; }

        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string? NombreConsumible { get; set; }

        [StringLength(100, ErrorMessage = "La marca no puede exceder 100 caracteres")]
        public string? Marca { get; set; }

        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres")]
        public string? Modelo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock actual debe ser mayor o igual a 0")]
        public int? StockActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock mÃ­nimo debe ser mayor o igual a 0")]
        public int? StockMinimo { get; set; }

        public string? DescripcionTecnica { get; set; }

        public long? IdInventario { get; set; }

        public long? IdTipoConsumible { get; set; }

        public long? IdEstadoConsumible { get; set; }
    }
}















