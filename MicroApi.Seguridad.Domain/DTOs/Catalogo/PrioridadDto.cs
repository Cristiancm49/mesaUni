using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Catalogo
{
    public class PrioridadDto
    {
        public long Id { get; set; }
        public string NombrePrioridad { get; set; } = string.Empty;
        public int TiempoRespuestaDias { get; set; }
        public int TiempoResolucionDias { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class PrioridadCreateDto
    {
        [Required(ErrorMessage = "El nombre de la prioridad es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombrePrioridad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tiempo de respuesta es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El tiempo de respuesta debe ser mayor a 0")]
        public int TiempoRespuestaDias { get; set; }

        [Required(ErrorMessage = "El tiempo de resolución es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El tiempo de resolución debe ser mayor a 0")]
        public int TiempoResolucionDias { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class PrioridadUpdateDto
    {
        [Required(ErrorMessage = "El ID de la prioridad es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombrePrioridad { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El tiempo de respuesta debe ser mayor a 0")]
        public int? TiempoRespuestaDias { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El tiempo de resolución debe ser mayor a 0")]
        public int? TiempoResolucionDias { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}














