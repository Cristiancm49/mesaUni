using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IComponenteService : IGenericService<Componente, ComponenteDto, ComponenteCreateDto, ComponenteUpdateDto>
    {
        Task<Chaira.MesaServicio.Domain.DTOs.Common.ApiResponseDto<IEnumerable<ComponenteDto>>> SearchAsync(string? query, int limit = 20);
    }
}













