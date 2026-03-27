using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    public class TrazabilidadCasoDto
    {
        public long Id { get; set; }
        public long IdCaso { get; set; }
        public DateTime FechaEvento { get; set; }
        public long IdUsuarioAccion { get; set; }
        public string? NombreUsuarioAccion { get; set; }
        public string TipoEvento { get; set; } = string.Empty;
        public string? Comentario { get; set; }
        public long? IdEstadoCaso { get; set; }
        public string? NombreEstadoCaso { get; set; }
        public long? IdAreaTecnica { get; set; }
        public string? NombreAreaTecnica { get; set; }
        public long? IdTecnicoAsignado { get; set; }
        public string? NombreTecnicoAsignado { get; set; }
        public List<IntervencionTecnicaDto> IntervencionesTecnicas { get; set; } = new();
    }

    public class TrazabilidadCasoCreateDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El ID del usuario de acción es requerido")]
        public long IdUsuarioAccion { get; set; }

        [Required(ErrorMessage = "El tipo de evento es requerido")]
        [StringLength(100, ErrorMessage = "El tipo de evento no puede exceder 100 caracteres")]
        public string TipoEvento { get; set; } = string.Empty;

        public string? Comentario { get; set; }

        public long? IdEstadoCaso { get; set; }

        public long? IdAreaTecnica { get; set; }

        public long? IdTecnicoAsignado { get; set; }
    }
}














