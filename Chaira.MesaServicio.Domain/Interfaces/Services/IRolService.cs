using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Acceso;
using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IRolService : IGenericService<Rol, RolDto, RolCreateDto, RolUpdateDto>
    {
        Task<ApiResponseDto<RolDto>> InactivarAsync(long id);
        Task<ApiResponseDto<RolDto>> ActivarAsync(long id);
    }
}













