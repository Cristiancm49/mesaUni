using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MicroApi.Seguridad.Api.Models
{
    public class EvidenciaUploadRequestDto
    {
        [Required]
        public string Modulo { get; set; } = string.Empty;

        [Required]
        public string Entidad { get; set; } = string.Empty;

        [Required]
        public long EntidadId { get; set; }

        public string? TipoEvidencia { get; set; }
        public string? Descripcion { get; set; }
        public long? IdCaso { get; set; }
        public long? IdIntervencionTecnica { get; set; }
        public long? IdUsuarioCarga { get; set; }

        [Required]
        public IFormFile? File { get; set; }
    }

    public class EvidenciaUploadMultipleRequestDto
    {
        [Required]
        public string Modulo { get; set; } = string.Empty;

        [Required]
        public string Entidad { get; set; } = string.Empty;

        [Required]
        public long EntidadId { get; set; }

        public string? TipoEvidencia { get; set; }
        public string? Descripcion { get; set; }
        public long? IdCaso { get; set; }
        public long? IdIntervencionTecnica { get; set; }
        public long? IdUsuarioCarga { get; set; }

        [Required]
        [MinLength(1)]
        public IFormFile[] Files { get; set; } = Array.Empty<IFormFile>();
    }

    public class EvidenciaUploadFilesRequestDto
    {
        public string? TipoEvidencia { get; set; }
        public string? Descripcion { get; set; }
        public long? IdUsuarioCarga { get; set; }

        [Required]
        [MinLength(1)]
        public IFormFile[] Files { get; set; } = Array.Empty<IFormFile>();
    }
}
