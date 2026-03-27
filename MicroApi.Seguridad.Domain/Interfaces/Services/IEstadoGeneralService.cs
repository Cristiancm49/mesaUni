using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IEstadoGeneralService : IGenericService<EstadoGeneral, EstadoGeneralDto, EstadoGeneralCreateDto, EstadoGeneralUpdateDto>
    {
    }
}
