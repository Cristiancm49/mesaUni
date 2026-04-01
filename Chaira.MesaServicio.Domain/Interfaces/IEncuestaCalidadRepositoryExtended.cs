using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IEncuestaCalidadRepositoryExtended
    {
        Task<EncuestaCalidadResponseDto> SpEncuestaCalidadCrearAsync(EncuestaCalidadCreateDto dto);
    }
}




