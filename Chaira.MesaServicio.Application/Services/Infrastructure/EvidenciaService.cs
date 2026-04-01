using Microsoft.Extensions.Configuration;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace Chaira.MesaServicio.Application.Services.Infrastructure
{
    public class EvidenciaService : IEvidenciaService
    {
        private static readonly Dictionary<string, HashSet<string>> AllowedModuleEntities = new(StringComparer.OrdinalIgnoreCase)
        {
            ["soporte"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "caso", "intervencion" },
            ["inventario"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "activo", "componente", "consumible" }
        };

        private readonly IEvidenciaRepository _repository;
        private readonly IConfiguration _configuration;

        public EvidenciaService(IEvidenciaRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<EvidenciaDto> UploadAsync(
            Stream stream,
            string fileName,
            string contentType,
            long tamanoBytes,
            EvidenciaUploadMetadataDto metadata,
            CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new InvalidOperationException("El archivo es requerido.");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new InvalidOperationException("El nombre del archivo es requerido.");
            }

            if (tamanoBytes <= 0)
            {
                throw new InvalidOperationException("El archivo no tiene contenido.");
            }

            ValidateModuleEntity(metadata.Modulo, metadata.Entidad);
            ValidateFile(fileName, tamanoBytes);

            var (preparedStream, hashSha256) = await PrepareStreamAndHashAsync(stream, cancellationToken);
            var normalizedMetadata = NormalizeMetadata(metadata);

            await using (preparedStream)
            {
                return await _repository.UploadAsync(
                    preparedStream,
                    fileName,
                    contentType,
                    tamanoBytes,
                    hashSha256,
                    normalizedMetadata,
                    cancellationToken);
            }
        }

        public Task<EvidenciaDto?> GetByIdAsync(string evidenciaId, CancellationToken cancellationToken = default)
            => _repository.GetByIdAsync(evidenciaId, cancellationToken);

        public Task<IReadOnlyList<EvidenciaDto>> GetByEntidadAsync(
            string modulo,
            string entidad,
            long entidadId,
            string? tipoEvidencia = null,
            CancellationToken cancellationToken = default)
        {
            ValidateModuleEntity(modulo, entidad);

            if (entidadId <= 0)
            {
                throw new InvalidOperationException("El id de entidad debe ser mayor que cero.");
            }

            return _repository.GetByEntidadAsync(
                modulo.ToLowerInvariant(),
                entidad.ToLowerInvariant(),
                entidadId,
                tipoEvidencia?.ToLowerInvariant(),
                cancellationToken);
        }

        public Task<IReadOnlyList<EvidenciasPorEntidadDto>> GetByEntidadesAsync(
            string modulo,
            string entidad,
            IEnumerable<long> entidadIds,
            CancellationToken cancellationToken = default)
        {
            ValidateModuleEntity(modulo, entidad);
            return _repository.GetByEntidadesAsync(
                modulo.ToLowerInvariant(),
                entidad.ToLowerInvariant(),
                entidadIds,
                cancellationToken);
        }

        public Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(
            string evidenciaId,
            CancellationToken cancellationToken = default)
            => _repository.DownloadAsync(evidenciaId, cancellationToken);

        public Task<bool> SoftDeleteAsync(
            string evidenciaId,
            long? idUsuarioEliminacion,
            CancellationToken cancellationToken = default)
            => _repository.SoftDeleteAsync(evidenciaId, idUsuarioEliminacion, cancellationToken);

        private void ValidateModuleEntity(string modulo, string entidad)
        {
            if (string.IsNullOrWhiteSpace(modulo) || string.IsNullOrWhiteSpace(entidad))
            {
                throw new InvalidOperationException("Modulo y entidad son obligatorios.");
            }

            if (!AllowedModuleEntities.TryGetValue(modulo, out var entities))
            {
                throw new InvalidOperationException("Modulo no permitido.");
            }

            if (!entities.Contains(entidad))
            {
                throw new InvalidOperationException("Entidad no permitida para el modulo indicado.");
            }
        }

        private void ValidateFile(string fileName, long tamanoBytes)
        {
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant() ?? string.Empty;
            var allowedExtensions = ResolveAllowedExtensions();
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Extension no permitida para el archivo.");
            }

            var maxSizeBytes = ResolveMaxFileSizeBytes();
            if (tamanoBytes > maxSizeBytes)
            {
                var maxMb = maxSizeBytes / (1024 * 1024);
                throw new InvalidOperationException($"El archivo excede el tamano maximo permitido ({maxMb} MB).");
            }
        }

        private HashSet<string> ResolveAllowedExtensions()
        {
            var configured = _configuration
                .GetSection("FileSettings:AllowedExtensions")
                .GetChildren()
                .Select(x => x.Value?.Trim().ToLowerInvariant())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.StartsWith('.') ? x : $".{x}")
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (configured.Count > 0)
            {
                return configured;
            }

            return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".gif"
            };
        }

        private long ResolveMaxFileSizeBytes()
        {
            var value = _configuration["FileSettings:MaxFileSizeMB"];
            if (int.TryParse(value, out var maxSizeMb) && maxSizeMb > 0)
            {
                return maxSizeMb * 1024L * 1024L;
            }

            return 10L * 1024L * 1024L;
        }

        private static EvidenciaUploadMetadataDto NormalizeMetadata(EvidenciaUploadMetadataDto metadata)
        {
            var modulo = metadata.Modulo.Trim().ToLowerInvariant();
            var entidad = metadata.Entidad.Trim().ToLowerInvariant();
            var tipoEvidencia = string.IsNullOrWhiteSpace(metadata.TipoEvidencia)
                ? "general"
                : metadata.TipoEvidencia.Trim().ToLowerInvariant();

            long? idCaso = metadata.IdCaso;
            long? idIntervencion = metadata.IdIntervencionTecnica;

            if (modulo == "soporte" && entidad == "caso" && idCaso == null)
            {
                idCaso = metadata.EntidadId;
            }

            if (modulo == "soporte" && entidad == "intervencion" && idIntervencion == null)
            {
                idIntervencion = metadata.EntidadId;
            }

            return new EvidenciaUploadMetadataDto
            {
                Modulo = modulo,
                Entidad = entidad,
                EntidadId = metadata.EntidadId,
                IdCaso = idCaso,
                IdIntervencionTecnica = idIntervencion,
                TipoEvidencia = tipoEvidencia,
                Descripcion = metadata.Descripcion?.Trim(),
                IdUsuarioCarga = metadata.IdUsuarioCarga
            };
        }

        private static async Task<(Stream Stream, string HashSha256)> PrepareStreamAndHashAsync(
            Stream inputStream,
            CancellationToken cancellationToken)
        {
            // Se usa MemoryStream porque el tamano maximo esta limitado por configuracion (10 MB por defecto).
            var memoryStream = new MemoryStream();
            await inputStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            using var sha256 = SHA256.Create();
            var hash = await sha256.ComputeHashAsync(memoryStream, cancellationToken);
            var hashString = ConvertToHex(hash);
            memoryStream.Position = 0;

            return (memoryStream, hashString);
        }

        private static string ConvertToHex(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}

