using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    public class CasoCambiarEstadoDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El nuevo estado es requerido")]
        public long IdEstadoNuevo { get; set; }

        [Required(ErrorMessage = "El ID del usuario que realiza la acción es requerido")]
        public long IdUsuarioAccion { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
        public string? Comentario { get; set; }
    }
}



