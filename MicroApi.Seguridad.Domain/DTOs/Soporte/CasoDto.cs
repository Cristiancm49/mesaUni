using System.ComponentModel.DataAnnotations;
using MicroApi.Seguridad.Domain.DTOs.Common;

namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    public class CasoDto
    {
        public long Id { get; set; }
        public string? NumeroCaso { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public long IdUsuarioReporta { get; set; }
        public string? NombreUsuarioReporta { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? CorreoContacto { get; set; }
        public long IdEstadoCaso { get; set; }
        public string? NombreEstadoCaso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaAceptacion { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public long IdTipoCaso { get; set; }
        public string? NombreTipoCaso { get; set; }
        public long? IdActivo { get; set; }
        public string? NombreActivo { get; set; }
        public string? CodigoPatrimonial { get; set; }
        public string? MarcaActivo { get; set; }
        public string? ModeloActivo { get; set; }
        public string? SerieActivo { get; set; }
        public string? CategoriaActivo { get; set; }
        public string? EstadoActivo { get; set; }
        public string? UbicacionActivo { get; set; }
        public long? IdAreaTecnica { get; set; }
        public string? NombreAreaTecnica { get; set; }
        public long IdPrioridad { get; set; }
        public string? NombrePrioridad { get; set; }
        public int? TiempoRespuestaDias { get; set; }
        public int? TiempoResolucionDias { get; set; }
        public long IdCanalIngreso { get; set; }
        public string? NombreCanalIngreso { get; set; }
        public long? IdTecnicoAsignado { get; set; }
        public string? NombreTecnicoAsignado { get; set; }
        public string? SlaStatus { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public List<EvidenciaDto> Evidencias { get; set; } = new();
    }

    public class CasoCreateDto
    {
        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario que reporta es requerido")]
        public long IdUsuarioReporta { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? TelefonoContacto { get; set; }

        [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? CorreoContacto { get; set; }

        [Required(ErrorMessage = "El ID del estado del caso es requerido")]
        public long IdEstadoCaso { get; set; }

        [Required(ErrorMessage = "El ID del tipo de caso es requerido")]
        public long IdTipoCaso { get; set; }

        public long? IdActivo { get; set; }

        public long? IdAreaTecnica { get; set; }

        [Required(ErrorMessage = "El ID de prioridad es requerido")]
        public long IdPrioridad { get; set; }

        [Required(ErrorMessage = "El ID del canal de ingreso es requerido")]
        public long IdCanalIngreso { get; set; }

        public long? IdTecnicoAsignado { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class CasoUpdateDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long Id { get; set; }

        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string? Descripcion { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? TelefonoContacto { get; set; }

        [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? CorreoContacto { get; set; }

        public long? IdEstadoCaso { get; set; }

        public DateTime? FechaAceptacion { get; set; }

        public DateTime? FechaResolucion { get; set; }

        public DateTime? FechaCierre { get; set; }

        public long? IdTipoCaso { get; set; }

        public long? IdActivo { get; set; }

        public long? IdAreaTecnica { get; set; }

        public long? IdPrioridad { get; set; }

        public long? IdCanalIngreso { get; set; }

        public long? IdTecnicoAsignado { get; set; }
    }

    public class CasoDetalleDto : CasoDto
    {
        public List<TrazabilidadCasoDto> Trazabilidades { get; set; } = new();
    }

    public class CasoFiltrosDto
    {
        public long? IdEstadoCaso { get; set; }
        public long? IdTecnico { get; set; }
        public long? IdAreaTecnica { get; set; }
        public long? IdTipoCaso { get; set; }
        public long? IdPrioridad { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? Busqueda { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class MisIncidenciasFiltrosDto
    {
        public string? Busqueda { get; set; }
        public string? EstadoGeneral { get; set; }
        public string? Prioridad { get; set; }
        public string? AreaTecnica { get; set; }
        public string? EstadoEspecifico { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? Tecnico { get; set; }
        public string? SlaStatus { get; set; }
    }
}














