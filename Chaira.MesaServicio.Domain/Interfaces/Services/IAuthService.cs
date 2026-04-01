using Chaira.MesaServicio.Domain.DTOs.Auth;

namespace Chaira.MesaServicio.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
}

