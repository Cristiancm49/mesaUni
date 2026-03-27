using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Catalogo
{
    public class EstadoCasoDto
    {
        public long Id { get; set; }
        public string NombreEstadoCaso { get; set; } = string.Empty;
        public string? DescripcionEstadoCaso { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstadoGeneral { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class EstadoCasoCreateDto
    {
        [Required(ErrorMessage = "El nombre del estado es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreEstadoCaso { get; set; } = string.Empty;

        public string? DescripcionEstadoCaso { get; set; }

        [Required(ErrorMessage = "El estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creacion es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class EstadoCasoUpdateDto
    {
        [Required(ErrorMessage = "El ID del estado es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombreEstadoCaso { get; set; }

        public string? DescripcionEstadoCaso { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}












