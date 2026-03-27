using MicroApi.Seguridad.Domain.DTOs.Common;

namespace MicroApi.Seguridad.Domain.DTOs.Reportes
{
    public class ConfiguracionAuditQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public string Usuario { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string Busqueda { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Equipo { get; set; } = string.Empty;
    }

    public class ConfiguracionAuditReportDto
    {
        public int TotalHistorico { get; set; }
        public PagedResponseDto<ConfiguracionAuditLogDto> Registros { get; set; } = new();
        public ConfiguracionAuditSummaryDto Estadisticas { get; set; } = new();
        public List<ConfiguracionAuditCountDto> AccionesMasComunes { get; set; } = new();
        public List<ConfiguracionAuditCountDto> EntidadesMasModificadas { get; set; } = new();
        public List<ConfiguracionAuditUserActivityDto> UsuariosMasActivos { get; set; } = new();
        public List<ConfiguracionAuditTrendDto> TendenciaCambios { get; set; } = new();
        public List<ConfiguracionAuditUserOptionDto> Usuarios { get; set; } = new();
        public List<string> Acciones { get; set; } = new();
        public List<string> Entidades { get; set; } = new();
    }

    public class ConfiguracionAuditSummaryDto
    {
        public int TotalCambios { get; set; }
        public int CambiosHoy { get; set; }
        public int CambiosEstaSemana { get; set; }
        public int CambiosEsteMes { get; set; }
        public int UsuariosActivos { get; set; }
    }

    public class ConfiguracionAuditCountDto
    {
        public string Clave { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class ConfiguracionAuditUserActivityDto
    {
        public string Usuario { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class ConfiguracionAuditTrendDto
    {
        public DateTime Fecha { get; set; }
        public string Etiqueta { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class ConfiguracionAuditUserOptionDto
    {
        public string Usuario { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
