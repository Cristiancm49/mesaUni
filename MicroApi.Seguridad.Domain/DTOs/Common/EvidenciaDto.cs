using System.ComponentModel.DataAnnotations;

namespace MicroApi.Seguridad.Domain.DTOs.Common
{
    public class EvidenciaDto
    {
        public string Id { get; set; } = string.Empty;
        public string FileId { get; set; } = string.Empty;
        public string HashSha256 { get; set; } = string.Empty;
        public string Modulo { get; set; } = string.Empty;
        public string Entidad { get; set; } = string.Empty;
        public long EntidadId { get; set; }
        public long? IdCaso { get; set; }
        public long? IdIntervencionTecnica { get; set; }
        public string TipoEvidencia { get; set; } = string.Empty;
        public string NombreArchivo { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long TamanoBytes { get; set; }
        public string? Descripcion { get; set; }
        public long? IdUsuarioCarga { get; set; }
        public DateTime FechaCarga { get; set; }
        public bool Eliminado { get; set; }
        public bool EsDuplicado { get; set; }
    }

    public class EvidenciaUploadMetadataDto
    {
        [Required]
        public string Modulo { get; set; } = string.Empty;

        [Required]
        public string Entidad { get; set; } = string.Empty;

        [Required]
        public long EntidadId { get; set; }

        public long? IdCaso { get; set; }
        public long? IdIntervencionTecnica { get; set; }
        public string? TipoEvidencia { get; set; }
        public string? Descripcion { get; set; }
        public long? IdUsuarioCarga { get; set; }
    }

    public class EvidenciasBatchQueryDto
    {
        [Required]
        [MinLength(1)]
        public List<long> EntidadIds { get; set; } = new();
    }

    public class EvidenciasPorEntidadDto
    {
        public long EntidadId { get; set; }
        public List<EvidenciaDto> Evidencias { get; set; } = new();
    }
}
