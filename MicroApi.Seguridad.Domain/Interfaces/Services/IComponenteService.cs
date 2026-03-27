using MicroApi.Seguridad.Domain.DTOs.Inventario;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IComponenteService : IGenericService<Componente, ComponenteDto, ComponenteCreateDto, ComponenteUpdateDto>
    {
        Task<MicroApi.Seguridad.Domain.DTOs.Common.ApiResponseDto<IEnumerable<ComponenteDto>>> SearchAsync(string? query, int limit = 20);
    }
}












