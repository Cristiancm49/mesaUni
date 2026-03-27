using MicroApi.Seguridad.Domain.DTOs.Inventario;
using MicroApi.Seguridad.Domain.Models.Inventario;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IConsumibleService : IGenericService<Consumible, ConsumibleDto, ConsumibleCreateDto, ConsumibleUpdateDto>
    {
        Task<MicroApi.Seguridad.Domain.DTOs.Common.ApiResponseDto<IEnumerable<ConsumibleDto>>> SearchAsync(string? query, int limit = 20);
    }
}












