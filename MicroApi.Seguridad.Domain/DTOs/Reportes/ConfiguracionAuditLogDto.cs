namespace MicroApi.Seguridad.Domain.DTOs.Reportes
{
    public class ConfiguracionAuditLogDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public long EntidadId { get; set; }
        public string EntidadNombre { get; set; } = string.Empty;
        public ConfiguracionAuditChangesDto Cambios { get; set; } = new();
        public string Ip { get; set; } = "N/D";
        public string UserAgent { get; set; } = "Backend";
        public string Detalles { get; set; } = string.Empty;
        public string Resultado { get; set; } = "success";
        public string Rol { get; set; } = "sistema";
        public string HostName { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
        public string CorrelationId { get; set; } = string.Empty;
        public string PrimaryKeyJson { get; set; } = string.Empty;
    }

    public class ConfiguracionAuditChangesDto
    {
        public object? Anterior { get; set; }
        public object? Nuevo { get; set; }
    }
}
