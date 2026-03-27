using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.DTOs.Acceso;
using MicroApi.Seguridad.Domain.Models.Acceso;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IRolService : IGenericService<Rol, RolDto, RolCreateDto, RolUpdateDto>
    {
        Task<ApiResponseDto<RolDto>> InactivarAsync(long id);
        Task<ApiResponseDto<RolDto>> ActivarAsync(long id);
    }
}












