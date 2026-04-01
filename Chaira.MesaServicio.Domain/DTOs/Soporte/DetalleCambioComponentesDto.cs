using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Soporte
{
    public class DetalleCambioComponentesDto
    {
        public long Id { get; set; }
        public long IdIntervencionTecnica { get; set; }
        public long IdComponente { get; set; }
        public string? NombreComponente { get; set; }
        public int Cantidad { get; set; }
        public string TipoCambio { get; set; } = string.Empty; // INSTALACION, RETIRO, REEMPLAZO
        public string? DescripcionCambio { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string? NombreUsuario { get; set; }
    }

    public class DetalleCambioComponentesCreateDto
    {
        [Required(ErrorMessage = "El ID de la intervenciÃ³n tÃ©cnica es requerido")]
        public long IdIntervencionTecnica { get; set; }

        [Required(ErrorMessage = "El ID del componente es requerido")]
        public long IdComponente { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El tipo de cambio es requerido")]
        [StringLength(20, ErrorMessage = "El tipo de cambio no puede exceder 20 caracteres")]
        [RegularExpression("^(INSTALACION|RETIRO|REEMPLAZO)$", ErrorMessage = "El tipo de cambio debe ser INSTALACION, RETIRO o REEMPLAZO")]
        public string TipoCambio { get; set; } = string.Empty;

        public string? DescripcionCambio { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }
}















