using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Catalogo
{
    public class TipoTrabajoDto
    {
        public long Id { get; set; }
        public string NombreTipoTrabajo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class TipoTrabajoCreateDto
    {
        [Required(ErrorMessage = "El nombre del tipo de trabajo es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreTipoTrabajo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class TipoTrabajoUpdateDto
    {
        [Required(ErrorMessage = "El ID del tipo de trabajo es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombreTipoTrabajo { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}















