using MicroApi.Seguridad.Domain.DTOs.Acceso;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Models.Acceso;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IUsuarioService : IGenericService<Usuario, UsuarioDto, UsuarioCreateDto, UsuarioUpdateDto>
    {
        Task<ApiResponseDto<UsuarioDto>> GetByEmailAsync(string email);
    }
}












