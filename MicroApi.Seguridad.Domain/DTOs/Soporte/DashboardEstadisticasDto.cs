namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    public class DashboardEstadisticasRequestDto
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public long? IdAreaTecnica { get; set; }
        public long? IdTecnico { get; set; }
    }

    public class DashboardEstadisticasDto
    {
        public ResumenGeneralDto ResumenGeneral { get; set; } = new();
        public List<CasosPorEstadoDto> CasosPorEstado { get; set; } = new();
        public List<CasosPorPrioridadDto> CasosPorPrioridad { get; set; } = new();
        public List<CasosPorTecnicoDto> CasosPorTecnico { get; set; } = new();
        public List<CasosPorAreaDto> CasosPorArea { get; set; } = new();
        public List<CasosPorTipoDto> CasosPorTipo { get; set; } = new();
        public List<TendenciaDiariaDto> TendenciaDiaria { get; set; } = new();
        public EstadoSLADto EstadoSLA { get; set; } = new();
    }

    public class ResumenGeneralDto
    {
        public int TotalCasos { get; set; }
        public int CasosAbiertos { get; set; }
        public int CasosResueltos { get; set; }
        public int CasosCerrados { get; set; }
        public int CasosSinAsignar { get; set; }
        public double? TiempoPromedioResolucionHoras { get; set; }
        public double? TiempoPromedioCierreHoras { get; set; }
    }

    public class CasosPorEstadoDto
    {
        public long IdEstadoCaso { get; set; }
        public string NombreEstadoCaso { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class CasosPorPrioridadDto
    {
        public long IdPrioridad { get; set; }
        public string NombrePrioridad { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int CasosAbiertos { get; set; }
        public int CasosCerrados { get; set; }
    }

    public class CasosPorTecnicoDto
    {
        public long IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public int CasosAsignados { get; set; }
        public int CasosResueltos { get; set; }
        public int CasosCerrados { get; set; }
        public int CasosEnProceso { get; set; }
        public double? TiempoPromedioResolucionHoras { get; set; }
    }

    public class CasosPorAreaDto
    {
        public long IdAreaTecnica { get; set; }
        public string NombreAreaTecnica { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int CasosAbiertos { get; set; }
        public int CasosCerrados { get; set; }
        public double? TiempoPromedioResolucionHoras { get; set; }
    }

    public class CasosPorTipoDto
    {
        public long IdTipoCaso { get; set; }
        public string NombreTipoCaso { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class TendenciaDiariaDto
    {
        public DateTime Fecha { get; set; }
        public int CasosCreados { get; set; }
        public int CasosResueltos { get; set; }
        public int CasosCerrados { get; set; }
    }

    public class EstadoSLADto
    {
        public int CasosEnTiempo { get; set; }
        public int CasosProximosVencer { get; set; }
        public int CasosVencidos { get; set; }
        public int CasosCerradosEnSLA { get; set; }
        public int CasosCerradosFueraSLA { get; set; }
    }
}



