using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    /// <summary>
    /// DTO para crear intervención técnica con SOLO diagnóstico (FASE 1)
    /// NO incluye componentes ni consumibles
    /// </summary>
    public class IntervencionDiagnosticoCreateDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El ID del tipo de trabajo es requerido")]
        public long IdTipoTrabajo { get; set; }

        [Required(ErrorMessage = "El diagnóstico es requerido")]
        [StringLength(2000, ErrorMessage = "El diagnóstico no puede exceder 2000 caracteres")]
        public string Diagnostico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario de acción es requerido")]
        public long IdUsuarioAccion { get; set; }
    }

    /// <summary>
    /// DTO para ejecutar intervención técnica DESPUÉS de aprobación (FASE 3)
    /// Incluye solución y componentes/consumibles usados
    /// </summary>
    public class IntervencionEjecutarDto
    {
        [Required(ErrorMessage = "El ID de la intervención es requerido")]
        public long IdIntervencionTecnica { get; set; }

        [Required(ErrorMessage = "La solución aplicada es requerida")]
        [StringLength(2000, ErrorMessage = "La solución aplicada no puede exceder 2000 caracteres")]
        public string SolucionAplicada { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario de acción es requerido")]
        public long IdUsuarioAccion { get; set; }

        /// <summary>
        /// Componentes realmente usados en la intervención
        /// </summary>
        public List<ComponenteIntervencionDto>? Componentes { get; set; }

        /// <summary>
        /// Consumibles realmente usados en la intervención
        /// </summary>
        public List<ConsumibleIntervencionDto>? Consumibles { get; set; }
    }

    /// <summary>
    /// DTO para corregir la solucion de una intervencion rechazada en revision administrativa
    /// </summary>
    public class IntervencionSolucionUpdateDto
    {
        [Required(ErrorMessage = "El ID de la intervención es requerido")]
        public long IdIntervencionTecnica { get; set; }

        [Required(ErrorMessage = "La solución aplicada es requerida")]
        [StringLength(2000, ErrorMessage = "La solución aplicada no puede exceder 2000 caracteres")]
        public string SolucionAplicada { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario de acción es requerido")]
        public long IdUsuarioAccion { get; set; }
    }

    // Nota: ComponenteIntervencionDto y ConsumibleIntervencionDto ya existen en IntervencionTecnicaDto.cs
}

