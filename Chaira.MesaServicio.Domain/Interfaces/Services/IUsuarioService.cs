using Chaira.MesaServicio.Domain.DTOs.Acceso;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IUsuarioService : IGenericService<Usuario, UsuarioDto, UsuarioCreateDto, UsuarioUpdateDto>
    {
        Task<ApiResponseDto<UsuarioDto>> GetByEmailAsync(string email);
    }
}













