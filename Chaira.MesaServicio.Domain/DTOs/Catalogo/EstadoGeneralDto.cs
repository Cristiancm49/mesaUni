using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Catalogo
{
    public class EstadoGeneralDto
    {
        public long Id { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class EstadoGeneralCreateDto
    {
        [Required(ErrorMessage = "El nombre del estado es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreEstado { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class EstadoGeneralUpdateDto
    {
        [Required(ErrorMessage = "El ID del estado es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombreEstado { get; set; }

        public string? Descripcion { get; set; }

        public bool? Activo { get; set; }
    }
}















