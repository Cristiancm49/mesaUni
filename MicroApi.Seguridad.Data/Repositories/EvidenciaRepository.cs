using MicroApi.Seguridad.Data.Models.Mongo;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class EvidenciaRepository : IEvidenciaRepository
    {
        private readonly IMongoCollection<EvidenciaDocument> _collection;
        private readonly GridFSBucket _bucket;

        public EvidenciaRepository(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<EvidenciaDocument>("evidencias");
            _bucket = new GridFSBucket(mongoDatabase, new GridFSBucketOptions
            {
                BucketName = "soportes"
            });

            EnsureIndexes();
        }

        public async Task<EvidenciaDto> UploadAsync(
            Stream stream,
            string fileName,
            string contentType,
            long tamanoBytes,
            string hashSha256,
            EvidenciaUploadMetadataDto metadata,
            CancellationToken cancellationToken = default)
        {
            var normalizedModulo = metadata.Modulo.ToLowerInvariant();
            var normalizedEntidad = metadata.Entidad.ToLowerInvariant();
            var normalizedTipoEvidencia = string.IsNullOrWhiteSpace(metadata.TipoEvidencia)
                ? "general"
                : metadata.TipoEvidencia.ToLowerInvariant();

            // Evitar duplicados por entidad + hash del contenido
            var duplicateFilter = Builders<EvidenciaDocument>.Filter.Eq(x => x.Modulo, normalizedModulo) &
                                  Builders<EvidenciaDocument>.Filter.Eq(x => x.Entidad, normalizedEntidad) &
                                  Builders<EvidenciaDocument>.Filter.Eq(x => x.EntidadId, metadata.EntidadId) &
                                  Builders<EvidenciaDocument>.Filter.Eq(x => x.HashSha256, hashSha256) &
                                  Builders<EvidenciaDocument>.Filter.Eq(x => x.Eliminado, false);

            var existing = await _collection.Find(duplicateFilter).FirstOrDefaultAsync(cancellationToken);
            if (existing != null)
            {
                return MapToDto(existing, esDuplicado: true);
            }

            var uploadOptions = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "modulo", normalizedModulo },
                    { "entidad", normalizedEntidad },
                    { "entidadId", metadata.EntidadId },
                    { "tipoEvidencia", normalizedTipoEvidencia },
                    { "hashSha256", hashSha256 }
                }
            };

            var fileObjectId = await _bucket.UploadFromStreamAsync(
                fileName,
                stream,
                uploadOptions,
                cancellationToken);

            var document = new EvidenciaDocument
            {
                FileId = fileObjectId.ToString(),
                HashSha256 = hashSha256,
                Modulo = normalizedModulo,
                Entidad = normalizedEntidad,
                EntidadId = metadata.EntidadId,
                IdCaso = metadata.IdCaso,
                IdIntervencionTecnica = metadata.IdIntervencionTecnica,
                TipoEvidencia = normalizedTipoEvidencia,
                NombreArchivo = fileName,
                ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType,
                TamanoBytes = tamanoBytes,
                Descripcion = metadata.Descripcion,
                IdUsuarioCarga = metadata.IdUsuarioCarga,
                FechaCarga = DateTime.UtcNow,
                Eliminado = false
            };

            await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);

            return MapToDto(document);
        }

        public async Task<EvidenciaDto?> GetByIdAsync(string evidenciaId, CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(evidenciaId, out _))
            {
                return null;
            }

            var filter = Builders<EvidenciaDocument>.Filter.Eq(x => x.Id, evidenciaId);
            var document = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            return document == null ? null : MapToDto(document);
        }

        public async Task<IReadOnlyList<EvidenciaDto>> GetByEntidadAsync(
            string modulo,
            string entidad,
            long entidadId,
            string? tipoEvidencia = null,
            CancellationToken cancellationToken = default)
        {
            var filterBuilder = Builders<EvidenciaDocument>.Filter;
            var filter = filterBuilder.Eq(x => x.Modulo, modulo.ToLowerInvariant()) &
                         filterBuilder.Eq(x => x.Entidad, entidad.ToLowerInvariant()) &
                         filterBuilder.Eq(x => x.EntidadId, entidadId) &
                         filterBuilder.Eq(x => x.Eliminado, false);

            if (!string.IsNullOrWhiteSpace(tipoEvidencia))
            {
                filter &= filterBuilder.Eq(x => x.TipoEvidencia, tipoEvidencia.ToLowerInvariant());
            }

            var documents = await _collection
                .Find(filter)
                .SortByDescending(x => x.FechaCarga)
                .ToListAsync(cancellationToken);

            return documents.Select(x => MapToDto(x)).ToList();
        }

        public async Task<IReadOnlyList<EvidenciasPorEntidadDto>> GetByEntidadesAsync(
            string modulo,
            string entidad,
            IEnumerable<long> entidadIds,
            CancellationToken cancellationToken = default)
        {
            var ids = entidadIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                return Array.Empty<EvidenciasPorEntidadDto>();
            }

            var filterBuilder = Builders<EvidenciaDocument>.Filter;
            var filter = filterBuilder.Eq(x => x.Modulo, modulo.ToLowerInvariant()) &
                         filterBuilder.Eq(x => x.Entidad, entidad.ToLowerInvariant()) &
                         filterBuilder.In(x => x.EntidadId, ids) &
                         filterBuilder.Eq(x => x.Eliminado, false);

            var documents = await _collection
                .Find(filter)
                .SortByDescending(x => x.FechaCarga)
                .ToListAsync(cancellationToken);

            var grouped = documents
                .GroupBy(x => x.EntidadId)
                .Select(g => new EvidenciasPorEntidadDto
                {
                    EntidadId = g.Key,
                    Evidencias = g.Select(x => MapToDto(x)).ToList()
                })
                .ToList();

            return grouped;
        }

        public async Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(
            string evidenciaId,
            CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(evidenciaId, out _))
            {
                return null;
            }

            var filter = Builders<EvidenciaDocument>.Filter.Eq(x => x.Id, evidenciaId) &
                         Builders<EvidenciaDocument>.Filter.Eq(x => x.Eliminado, false);
            var document = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            if (document == null || !ObjectId.TryParse(document.FileId, out var fileObjectId))
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            await _bucket.DownloadToStreamAsync(fileObjectId, memoryStream, cancellationToken: cancellationToken);
            memoryStream.Position = 0;

            return (memoryStream, document.NombreArchivo, document.ContentType);
        }

        public async Task<bool> SoftDeleteAsync(
            string evidenciaId,
            long? idUsuarioEliminacion,
            CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(evidenciaId, out _))
            {
                return false;
            }

            var filter = Builders<EvidenciaDocument>.Filter.Eq(x => x.Id, evidenciaId) &
                         Builders<EvidenciaDocument>.Filter.Eq(x => x.Eliminado, false);
            var update = Builders<EvidenciaDocument>.Update
                .Set(x => x.Eliminado, true)
                .Set(x => x.FechaEliminacion, DateTime.UtcNow)
                .Set(x => x.IdUsuarioEliminacion, idUsuarioEliminacion);

            var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            return result.ModifiedCount > 0;
        }

        private static EvidenciaDto MapToDto(EvidenciaDocument document, bool esDuplicado = false)
        {
            return new EvidenciaDto
            {
                Id = document.Id,
                FileId = document.FileId,
                HashSha256 = document.HashSha256,
                Modulo = document.Modulo,
                Entidad = document.Entidad,
                EntidadId = document.EntidadId,
                IdCaso = document.IdCaso,
                IdIntervencionTecnica = document.IdIntervencionTecnica,
                TipoEvidencia = document.TipoEvidencia,
                NombreArchivo = document.NombreArchivo,
                ContentType = document.ContentType,
                TamanoBytes = document.TamanoBytes,
                Descripcion = document.Descripcion,
                IdUsuarioCarga = document.IdUsuarioCarga,
                FechaCarga = document.FechaCarga,
                Eliminado = document.Eliminado,
                EsDuplicado = esDuplicado
            };
        }

        private void EnsureIndexes()
        {
            var indexKeys = Builders<EvidenciaDocument>.IndexKeys
                .Ascending(x => x.Modulo)
                .Ascending(x => x.Entidad)
                .Ascending(x => x.EntidadId)
                .Ascending(x => x.HashSha256)
                .Ascending(x => x.Eliminado)
                .Descending(x => x.FechaCarga);

            _collection.Indexes.CreateOne(new CreateIndexModel<EvidenciaDocument>(indexKeys));
        }
    }
}
