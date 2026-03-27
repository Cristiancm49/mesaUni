using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IRevisionAdmiRepositoryExtended
    {
        Task<RevisionAdmiResponseDto> SpRevisionAdmiProcesarAsync(RevisionAdmiCreateDto dto);
        Task<RevisionAdmiQueueDto> SpRevisionAdmiObtenerBandejaAsync();
    }
}



