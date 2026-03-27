namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    /// <summary>
    /// DTO para actualizar el diagnóstico de una intervención existente
    /// (usado cuando el admin rechaza y el técnico necesita corregir)
    /// </summary>
    public class IntervencionDiagnosticoUpdateDto
    {
        public long IdIntervencionTecnica { get; set; }
        public long? IdTipoTrabajo { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public long IdUsuarioAccion { get; set; }
    }
}
