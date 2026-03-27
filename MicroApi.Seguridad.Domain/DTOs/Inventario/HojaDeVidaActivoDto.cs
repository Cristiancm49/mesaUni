using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Inventario
{
    public class HojaDeVidaActivoDto
    {
        public long Id { get; set; }
        public long IdActivo { get; set; }
        public string? NombreActivo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DetalleRegistro { get; set; } = string.Empty;
        public string? TipoEvento { get; set; }
        public long? IdCaso { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string? NombreUsuario { get; set; }
    }

    public class HojaDeVidaActivoCreateDto
    {
        [Required(ErrorMessage = "El ID del activo es requerido")]
        public long IdActivo { get; set; }

        [Required(ErrorMessage = "El detalle del registro es requerido")]
        public string DetalleRegistro { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El tipo de evento no puede exceder 100 caracteres")]
        public string? TipoEvento { get; set; }

        public long? IdCaso { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }
}














