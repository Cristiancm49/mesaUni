using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public static class RevisionTipos
    {
        public const string Diagnostico = "DIAGNOSTICO";
        public const string Solucion = "SOLUCION";
    }

    public class RevisionAdmiDto
    {
        public long Id { get; set; }
        public long IdIntervencionTecnica { get; set; }
        public bool Aprobado { get; set; }
        public string? ObservacionRevision { get; set; }
        public string TipoRevision { get; set; } = RevisionTipos.Diagnostico;
        public DateTime FechaRegistro { get; set; }
        public long IdUsuarioCreacion { get; set; }
    }

    public class RevisionAdmiCreateDto
    {
        [Required(ErrorMessage = "El ID de la intervenciÃ³n tÃ©cnica es requerido")]
        public long IdIntervencionTecnica { get; set; }

        [Required(ErrorMessage = "El estado de aprobaciÃ³n es requerido")]
        public bool Aprobado { get; set; }

        [Required(ErrorMessage = "El tipo de revision es requerido")]
        public string TipoRevision { get; set; } = RevisionTipos.Diagnostico;

        public string? ObservacionRevision { get; set; }

        [Required(ErrorMessage = "El ID del usuario que realiza la revisiÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class RevisionAdmiUpdateDto
    {
        [Required]
        public long Id { get; set; }

        public bool? Aprobado { get; set; }
        public string? ObservacionRevision { get; set; }
        public string? TipoRevision { get; set; }
    }

    public class RevisionAdmiResponseDto
    {
        public long Id { get; set; }
        public long IdIntervencionTecnica { get; set; }
        public string EstadoAprobacion { get; set; } = string.Empty;
        public string TipoRevision { get; set; } = RevisionTipos.Diagnostico;
        public string? Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string NombreRevisor { get; set; } = string.Empty;
        public long IdCaso { get; set; }
        public string NumeroCaso { get; set; } = string.Empty;
    }

    public class RevisionAdmiQueueItemDto
    {
        public long IdIntervencionTecnica { get; set; }
        public long IdCaso { get; set; }
        public string NumeroCaso { get; set; } = string.Empty;
        public string DescripcionCaso { get; set; } = string.Empty;
        public string TipoRevision { get; set; } = RevisionTipos.Diagnostico;
        public string NombreTipoTrabajo { get; set; } = string.Empty;
        public string NombreEstadoIntervencion { get; set; } = string.Empty;
        public string NombreEstadoCaso { get; set; } = string.Empty;
        public string NombrePrioridad { get; set; } = string.Empty;
        public string? NombreTecnicoAsignado { get; set; }
        public string? NombreAreaTecnica { get; set; }
        public string? NombreUsuarioReporta { get; set; }
        public string? NombreActivo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Diagnostico { get; set; }
        public string? SolucionAplicada { get; set; }
        public long CantidadComponentes { get; set; }
        public long CantidadConsumibles { get; set; }
        public long? IdRevisionAdmi { get; set; }
        public bool? Aprobado { get; set; }
        public string? EstadoAprobacion { get; set; }
        public string? ObservacionRevision { get; set; }
        public DateTime? FechaRevision { get; set; }
        public string? NombreRevisor { get; set; }
    }

    public class RevisionAdmiQueueDto
    {
        public List<RevisionAdmiQueueItemDto> PendientesDiagnostico { get; set; } = new();
        public List<RevisionAdmiQueueItemDto> PendientesSolucion { get; set; } = new();
        public List<RevisionAdmiQueueItemDto> Historico { get; set; } = new();
    }
}

