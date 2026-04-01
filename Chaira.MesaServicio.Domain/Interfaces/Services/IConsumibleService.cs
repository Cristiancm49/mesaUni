using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Models.Inventario;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IConsumibleService : IGenericService<Consumible, ConsumibleDto, ConsumibleCreateDto, ConsumibleUpdateDto>
    {
        Task<Chaira.MesaServicio.Domain.DTOs.Common.ApiResponseDto<IEnumerable<ConsumibleDto>>> SearchAsync(string? query, int limit = 20);
    }
}













