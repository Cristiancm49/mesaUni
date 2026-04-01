using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Catalogo
{
    public class CategoriaActivoDto
    {
        public long Id { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class CategoriaActivoCreateDto
    {
        [Required(ErrorMessage = "El nombre de la categorÃ­a es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NombreCategoria { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El ID del estado general es requerido")]
        public long IdEstadoGeneral { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creaciÃ³n es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }

    public class CategoriaActivoUpdateDto
    {
        [Required(ErrorMessage = "El ID de la categorÃ­a es requerido")]
        public long Id { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? NombreCategoria { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}















