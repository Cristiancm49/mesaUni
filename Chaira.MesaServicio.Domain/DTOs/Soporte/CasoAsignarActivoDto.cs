using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public class CasoAsignarActivoDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El ID del activo es requerido")]
        public long IdActivo { get; set; }

        [Required(ErrorMessage = "El ID del usuario que realiza la acciÃ³n es requerido")]
        public long IdUsuarioAccion { get; set; }
    }
}




