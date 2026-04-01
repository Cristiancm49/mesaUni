using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chaira.MesaServicio.Infrastructure.Models.Mongo
{
    public class EvidenciaDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string FileId { get; set; } = string.Empty;

        [BsonElement("hashSha256")]
        public string HashSha256 { get; set; } = string.Empty;

        [BsonElement("modulo")]
        public string Modulo { get; set; } = string.Empty;

        [BsonElement("entidad")]
        public string Entidad { get; set; } = string.Empty;

        [BsonElement("entidadId")]
        public long EntidadId { get; set; }

        [BsonElement("idCaso")]
        [BsonIgnoreIfNull]
        public long? IdCaso { get; set; }

        [BsonElement("idIntervencionTecnica")]
        [BsonIgnoreIfNull]
        public long? IdIntervencionTecnica { get; set; }

        [BsonElement("tipoEvidencia")]
        public string TipoEvidencia { get; set; } = "general";

        [BsonElement("nombreArchivo")]
        public string NombreArchivo { get; set; } = string.Empty;

        [BsonElement("contentType")]
        public string ContentType { get; set; } = "application/octet-stream";

        [BsonElement("tamanoBytes")]
        public long TamanoBytes { get; set; }

        [BsonElement("descripcion")]
        [BsonIgnoreIfNull]
        public string? Descripcion { get; set; }

        [BsonElement("idUsuarioCarga")]
        [BsonIgnoreIfNull]
        public long? IdUsuarioCarga { get; set; }

        [BsonElement("fechaCarga")]
        public DateTime FechaCarga { get; set; } = DateTime.UtcNow;

        [BsonElement("eliminado")]
        public bool Eliminado { get; set; }

        [BsonElement("fechaEliminacion")]
        [BsonIgnoreIfNull]
        public DateTime? FechaEliminacion { get; set; }

        [BsonElement("idUsuarioEliminacion")]
        [BsonIgnoreIfNull]
        public long? IdUsuarioEliminacion { get; set; }
    }
}

