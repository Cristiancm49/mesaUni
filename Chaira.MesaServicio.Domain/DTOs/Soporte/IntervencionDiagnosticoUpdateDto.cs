namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    /// <summary>
    /// DTO para actualizar el diagnÃ³stico de una intervenciÃ³n existente
    /// (usado cuando el admin rechaza y el tÃ©cnico necesita corregir)
    /// </summary>
    public class IntervencionDiagnosticoUpdateDto
    {
        public long IdIntervencionTecnica { get; set; }
        public long? IdTipoTrabajo { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public long IdUsuarioAccion { get; set; }
    }
}

