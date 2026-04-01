namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public class TecnicoIncidenciasResumenDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Area { get; set; } = "No definida";
        public int IncidenciasAsignadas { get; set; }
        public int IncidenciasActivas { get; set; }
        public int IncidenciasPendientes { get; set; }
        public int IncidenciasEnProceso { get; set; }
        public int IncidenciasCriticas { get; set; }
        public int IncidenciasAltaPrioridad { get; set; }
        public int IncidenciasVencidas { get; set; }
        public int IncidenciasCerradas { get; set; }
        public double PromedioResolucion { get; set; }
        public double CumplimientoSLA { get; set; }
    }
}

