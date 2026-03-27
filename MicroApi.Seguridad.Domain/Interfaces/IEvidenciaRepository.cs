using MicroApi.Seguridad.Domain.DTOs.Common;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IEvidenciaRepository
    {
        Task<EvidenciaDto> UploadAsync(
            Stream stream,
            string fileName,
            string contentType,
            long tamanoBytes,
            string hashSha256,
            EvidenciaUploadMetadataDto metadata,
            CancellationToken cancellationToken = default);

        Task<EvidenciaDto?> GetByIdAsync(string evidenciaId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EvidenciaDto>> GetByEntidadAsync(
            string modulo,
            string entidad,
            long entidadId,
            string? tipoEvidencia = null,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EvidenciasPorEntidadDto>> GetByEntidadesAsync(
            string modulo,
            string entidad,
            IEnumerable<long> entidadIds,
            CancellationToken cancellationToken = default);

        Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(
            string evidenciaId,
            CancellationToken cancellationToken = default);

        Task<bool> SoftDeleteAsync(
            string evidenciaId,
            long? idUsuarioEliminacion,
            CancellationToken cancellationToken = default);
    }
}
