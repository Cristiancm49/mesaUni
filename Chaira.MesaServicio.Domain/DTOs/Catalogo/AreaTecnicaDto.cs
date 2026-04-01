using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Catalogo
{
    public class AreaTecnicaDto
    {
        public long Id { get; set; }
        public string NombreAreaTecnica { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public long IdEncargado { get; set; }
        public string? NombreEncargado { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class AreaTecnicaCreateDto
    {
        [Required(ErrorMessage = "El nombre del Ã¡rea tÃ©cnica es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreAreaTecnica { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El ID del encargado es requerido")]
        public long IdEncargado { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class AreaTecnicaUpdateDto
    {
        [Required(ErrorMessage = "El ID del Ã¡rea tÃ©cnica es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombreAreaTecnica { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEncargado { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}















