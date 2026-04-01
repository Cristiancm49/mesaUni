using System.ComponentModel.DataAnnotations;

namespace Chaira.MesaServicio.Domain.DTOs.Catalogo
{
    public class SedeDto
    {
        public long Id { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public long IdEstadoGeneral { get; set; }
        public string? NombreEstado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class SedeCreateDto
    {
        [Required(ErrorMessage = "El nombre de la sede es requerido")]
        [StringLength(150)]
        public string NombreSede { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required]
        public long IdEstadoGeneral { get; set; }

        [Required]
        public long IdUsuarioCreacion { get; set; }
    }

    public class SedeUpdateDto
    {
        [Required]
        public long Id { get; set; }

        [StringLength(150)]
        public string? NombreSede { get; set; }

        public string? Descripcion { get; set; }

        public long? IdEstadoGeneral { get; set; }
    }
}











