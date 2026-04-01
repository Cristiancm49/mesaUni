using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IRevisionAdmiRepositoryExtended
    {
        Task<RevisionAdmiResponseDto> SpRevisionAdmiProcesarAsync(RevisionAdmiCreateDto dto);
        Task<RevisionAdmiQueueDto> SpRevisionAdmiObtenerBandejaAsync();
    }
}




