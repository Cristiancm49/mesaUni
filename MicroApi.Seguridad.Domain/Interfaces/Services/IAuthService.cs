using MicroApi.Seguridad.Domain.DTOs.Auth;

namespace MicroApi.Seguridad.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}
