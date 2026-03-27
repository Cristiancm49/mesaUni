using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Catalogo
{
    public class TipoConsumibleDto
    {
        public long Id { get; set; }
        public string NombreTipo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class TipoConsumibleCreateDto
    {
        [Required(ErrorMessage = "El nombre del tipo es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreTipo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class TipoConsumibleUpdateDto
    {
        [Required(ErrorMessage = "El ID del tipo es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombreTipo { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}














