using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public class CasoEscalarDto
    {
        [Required(ErrorMessage = "El ID del caso es requerido")]
        public long IdCaso { get; set; }

        [Required(ErrorMessage = "El Ã¡rea tÃ©cnica destino es requerida")]
        public long IdAreaTecnicaNueva { get; set; }

        public long? IdTecnicoAsignadoNuevo { get; set; }

        [Required(ErrorMessage = "El motivo del escalamiento es requerido")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "El motivo debe tener entre 10 y 500 caracteres")]
        public string MotivoEscalamiento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ID del usuario que realiza la acciÃ³n es requerido")]
        public long IdUsuarioAccion { get; set; }
    }
}




