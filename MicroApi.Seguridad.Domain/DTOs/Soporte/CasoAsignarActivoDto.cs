using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    public class CasoAsignarActivoDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El ID del activo es requerido")]
        public long IdActivo { get; set; }

        [Required(ErrorMessage = "El ID del usuario que realiza la acción es requerido")]
        public long IdUsuarioAccion { get; set; }
    }
}



