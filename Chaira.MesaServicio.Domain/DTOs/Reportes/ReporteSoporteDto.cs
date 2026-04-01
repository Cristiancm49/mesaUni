namespace Chaira.MesaServicio.Domain.DTOs.Reportes
{
    public class ReporteCasoDto
    {
        public long Id { get; set; }
        public long IdUsuarioReporta { get; set; }
        public long? IdTecnicoAsignado { get; set; }
        public long IdEstadoCaso { get; set; }
        public long IdTipoCaso { get; set; }
        public long? IdAreaTecnica { get; set; }
        public long IdPrioridad { get; set; }
        public long? IdActivo { get; set; }
        public string NumeroCaso { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string EstadoEspecifico { get; set; } = string.Empty;
        public string TipoCaso { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string UsuarioReporta { get; set; } = string.Empty;
        public string NombreTecnico { get; set; } = string.Empty;
        public string TecnicoAsignado { get; set; } = string.Empty;
        public string AreaTecnica { get; set; } = string.Empty;
        public string NombreAreaTecnica { get; set; } = string.Empty;
        public int DiasAbierto { get; set; }
        public double? TiempoResolucion { get; set; }
        public double TiempoAbiertoHoras { get; set; }
        public double? Satisfaccion { get; set; }
        public string ActivoAfectado { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string CodigoPatrimonial { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public bool Escalado { get; set; }
        public string? NivelEscalado { get; set; }
        public string? MotivoEscalado { get; set; }
        public bool Retrasado { get; set; }
        public int DiasRetraso { get; set; }
    }

    public class ReporteEncuestaDto
    {
        public long Id { get; set; }
        public long IdCaso { get; set; }
        public string CasoId { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreTecnico { get; set; } = string.Empty;
        public string TecnicoAsignado { get; set; } = string.Empty;
        public string AreaTecnica { get; set; } = string.Empty;
        public string TipoCaso { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public DateTime FechaEncuesta { get; set; }
        public int SatisfaccionGeneral { get; set; }
        public double PromedioRespuestas { get; set; }
        public double TiempoResolucion { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public List<ReporteEncuestaRespuestaDto> Respuestas { get; set; } = new();
    }

    public class ReporteEncuestaRespuestaDto
    {
        public string Pregunta { get; set; } = string.Empty;
        public string Respuesta { get; set; } = string.Empty;
        public int ValorNumerico { get; set; }
    }
}

