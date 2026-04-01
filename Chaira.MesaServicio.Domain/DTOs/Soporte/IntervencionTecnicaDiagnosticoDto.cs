using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    /// <summary>
    /// DTO para crear intervenciÃ³n tÃ©cnica con SOLO diagnÃ³stico (FASE 1)
    /// NO incluye componentes ni consumibles
    /// </summary>
    public class IntervencionDiagnosticoCreateDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El ID del tipo de trabajo es requerido")]
        public long IdTipoTrabajo { get; set; }

        [Required(ErrorMessage = "El diagnÃ³stico es requerido")]
        [StringLength(2000, ErrorMessage = "El diagnÃ³stico no puede exceder 2000 caracteres")]
        public string Diagnostico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario de acciÃ³n es requerido")]
        public long IdUsuarioAccion { get; set; }
    }

    /// <summary>
    /// DTO para ejecutar intervenciÃ³n tÃ©cnica DESPUÃ‰S de aprobaciÃ³n (FASE 3)
    /// Incluye soluciÃ³n y componentes/consumibles usados
    /// </summary>
    public class IntervencionEjecutarDto
    {
        [Required(ErrorMessage = "El ID de la intervenciÃ³n es requerido")]
        public long IdIntervencionTecnica { get; set; }

        [Required(ErrorMessage = "La soluciÃ³n aplicada es requerida")]
        [StringLength(2000, ErrorMessage = "La soluciÃ³n aplicada no puede exceder 2000 caracteres")]
        public string SolucionAplicada { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario de acciÃ³n es requerido")]
        public long IdUsuarioAccion { get; set; }

        /// <summary>
        /// Componentes realmente usados en la intervenciÃ³n
        /// </summary>
        public List<ComponenteIntervencionDto>? Componentes { get; set; }

        /// <summary>
        /// Consumibles realmente usados en la intervenciÃ³n
        /// </summary>
        public List<ConsumibleIntervencionDto>? Consumibles { get; set; }
    }

    /// <summary>
    /// DTO para corregir la solucion de una intervencion rechazada en revision administrativa
    /// </summary>
    public class IntervencionSolucionUpdateDto
    {
        [Required(ErrorMessage = "El ID de la intervenciÃ³n es requerido")]
        public long IdIntervencionTecnica { get; set; }

        [Required(ErrorMessage = "La soluciÃ³n aplicada es requerida")]
        [StringLength(2000, ErrorMessage = "La soluciÃ³n aplicada no puede exceder 2000 caracteres")]
        public string SolucionAplicada { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario de acciÃ³n es requerido")]
        public long IdUsuarioAccion { get; set; }
    }

    // Nota: ComponenteIntervencionDto y ConsumibleIntervencionDto ya existen en IntervencionTecnicaDto.cs
}


