using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public class CasoAsignarTecnicoDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El ID del tÃ©cnico es requerido")]
        public long IdTecnicoAsignado { get; set; }

        [Required(ErrorMessage = "El ID del usuario que realiza la acciÃ³n es requerido")]
        public long IdUsuarioAccion { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
        public string? Comentario { get; set; }
    }
}




