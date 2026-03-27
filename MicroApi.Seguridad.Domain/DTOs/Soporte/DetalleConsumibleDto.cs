using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Soporte
{
    public class DetalleConsumibleDto
    {
        public long Id { get; set; }
        public long IdIntervencionTecnica { get; set; }
        public long IdConsumible { get; set; }
        public string? NombreConsumible { get; set; }
        public int Cantidad { get; set; }
        public string? DescripcionUso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long IdUsuarioCreacion { get; set; }
        public string? NombreUsuario { get; set; }
    }

    public class DetalleConsumibleCreateDto
    {
        [Required(ErrorMessage = "El ID de la intervención técnica es requerido")]
        public long IdIntervencionTecnica { get; set; }

        [Required(ErrorMessage = "El ID del consumible es requerido")]
        public long IdConsumible { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        public string? DescripcionUso { get; set; }

        [Required(ErrorMessage = "El ID del usuario de creación es requerido")]
        public long IdUsuarioCreacion { get; set; }
    }
}














