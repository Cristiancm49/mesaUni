namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    public class CasoHistorialCompletoDto
    {
        public CasoDetalleCompletoDto Caso { get; set; } = new();
        public List<TrazabilidadDto> Trazabilidad { get; set; } = new();
        public List<IntervencionTecnicaDetalleDto> Intervenciones { get; set; } = new();
        public List<ComponenteUsadoDto> Componentes { get; set; } = new();
        public List<ConsumibleUsadoDto> Consumibles { get; set; } = new();
        public List<RevisionAdmiDetalleDto> Revisiones { get; set; } = new();
        public List<EncuestaCalidadDetalleDto> Encuestas { get; set; } = new();
    }

    public class CasoDetalleCompletoDto
    {
        public long IdCaso { get; set; }
        public string NumeroCaso { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public long IdUsuarioReporta { get; set; }
        public string NombreUsuarioReporta { get; set; } = string.Empty;
        public string? EmailUsuarioReporta { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? CorreoContacto { get; set; }
        public long? IdTecnicoAsignado { get; set; }
        public string? NombreTecnicoAsignado { get; set; }
        public string? EmailTecnicoAsignado { get; set; }
        public long IdEstadoCaso { get; set; }
        public string NombreEstadoCaso { get; set; } = string.Empty;
        public long IdPrioridad { get; set; }
        public string NombrePrioridad { get; set; } = string.Empty;
        public long IdTipoCaso { get; set; }
        public string NombreTipoCaso { get; set; } = string.Empty;
        public long IdCanalIngreso { get; set; }
        public string NombreCanal { get; set; } = string.Empty;
        public long? IdAreaTecnica { get; set; }
        public string? NombreAreaTecnica { get; set; }
        public long? IdActivo { get; set; }
        public string? NombreActivo { get; set; }
        public string? SerialActivo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaAceptacion { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string NombreUsuarioCreacion { get; set; } = string.Empty;
    }

    public class TrazabilidadDto
    {
        public long Id { get; set; }
        public DateTime FechaEvento { get; set; }
        public string TipoEvento { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;
        public long IdUsuarioAccion { get; set; }
        public string NombreUsuarioAccion { get; set; } = string.Empty;
        public string? EmailUsuarioAccion { get; set; }
        public long? IdEstadoCaso { get; set; }
        public string? NombreEstadoCaso { get; set; }
        public long? IdAreaTecnica { get; set; }
        public string? NombreAreaTecnica { get; set; }
        public long? IdTecnicoAsignado { get; set; }
        public string? NombreTecnicoAsignado { get; set; }
    }

    public class IntervencionTecnicaDetalleDto
    {
        public long Id { get; set; }
        public long IdCaso { get; set; }
        public long IdTipoTrabajo { get; set; }
        public string NombreTipoTrabajo { get; set; } = string.Empty;
        public long IdEstadoIntervencion { get; set; }
        public string NombreEstadoIntervencion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string? SolucionAplicada { get; set; }
        public long IdUsuarioAccion { get; set; }
        public string NombreTecnico { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int CantidadComponentes { get; set; }
        public int CantidadConsumibles { get; set; }
    }

    public class ComponenteUsadoDto
    {
        public long Id { get; set; }
        public long IdIntervencionTecnica { get; set; }
        public long IdComponente { get; set; }
        public string NombreComponente { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int Cantidad { get; set; }
        public string TipoCambio { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class ConsumibleUsadoDto
    {
        public long Id { get; set; }
        public long IdIntervencionTecnica { get; set; }
        public long IdConsumible { get; set; }
        public string NombreConsumible { get; set; } = string.Empty;
        public string? Marca { get; set; }
        public int Cantidad { get; set; }
        public string? DescripcionUso { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class RevisionAdmiDetalleDto
    {
        public long Id { get; set; }
        public long IdIntervencionTecnica { get; set; }
        public string EstadoAprobacion { get; set; } = string.Empty;
        public string TipoRevision { get; set; } = RevisionTipos.Diagnostico;
        public string? Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string NombreRevisor { get; set; } = string.Empty;
    }

    public class EncuestaCalidadDetalleDto
    {
        public long Id { get; set; }
        public long IdCaso { get; set; }
        public DateTime FechaEncuesta { get; set; }
        public string? Observaciones { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int CantidadRespuestas { get; set; }
    }
}


