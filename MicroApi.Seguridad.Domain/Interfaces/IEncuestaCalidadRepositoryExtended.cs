using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IEncuestaCalidadRepositoryExtended
    {
        Task<EncuestaCalidadResponseDto> SpEncuestaCalidadCrearAsync(EncuestaCalidadCreateDto dto);
    }
}



