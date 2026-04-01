using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Chaira.MesaServicio.Application.Auth.Models;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Security;

namespace Chaira.MesaServicio.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loginDto = request.Credentials;
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);

            if (usuario == null)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };
            }

            if (string.IsNullOrEmpty(loginDto.Password))
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Password requerido"
                };
            }

            var usuarioAuth = new UsuarioAuthDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                IdRol = usuario.IdRol,
                Rol = AppRoles.Normalize(usuario.Rol?.NombreRol)
            };

            var expiryMinutes = ResolveTokenExpiryMinutes();
            var token = GenerateJwtToken(usuarioAuth, expiryMinutes);
            var expiration = DateTime.UtcNow.AddMinutes(expiryMinutes);

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login exitoso",
                Token = token,
                Expiration = expiration,
                Usuario = usuarioAuth
            };
        }
        catch (Exception ex)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = $"Error en login: {ex.Message}"
            };
        }
    }

    private string GenerateJwtToken(UsuarioAuthDto usuario, int expiryMinutes)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey no configurada");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Name, usuario.NombreCompleto),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim("IdRol", usuario.IdRol.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int ResolveTokenExpiryMinutes()
    {
        const int defaultExpiryMinutes = 1440;

        var envExpiryMinutes = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES");
        if (int.TryParse(envExpiryMinutes, out var parsedMinutes) && parsedMinutes > 0)
            return parsedMinutes;

        var envExpirationHours = Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS");
        if (int.TryParse(envExpirationHours, out var parsedHours) && parsedHours > 0)
            return parsedHours * 60;

        var configExpiryMinutes = _configuration["JwtSettings:ExpiryMinutes"];
        if (int.TryParse(configExpiryMinutes, out var parsedConfigMinutes) && parsedConfigMinutes > 0)
            return parsedConfigMinutes;

        return defaultExpiryMinutes;
    }
}

