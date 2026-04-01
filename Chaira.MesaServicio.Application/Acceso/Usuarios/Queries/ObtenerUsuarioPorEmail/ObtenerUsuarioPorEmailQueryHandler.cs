using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Mappers;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorEmail;

public sealed class ObtenerUsuarioPorEmailQueryHandler : IRequestHandler<ObtenerUsuarioPorEmailQuery, ApiResponseDto<UsuarioDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ObtenerUsuarioPorEmailQueryHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ApiResponseDto<UsuarioDto>> Handle(ObtenerUsuarioPorEmailQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

        if (usuario is null)
        {
            return ApiResponseDto<UsuarioDto>.FailResponse($"Usuario con email '{request.Email}' no encontrado");
        }

        return ApiResponseDto<UsuarioDto>.SuccessResponse(UsuarioMapper.ToDto(usuario), "Registro obtenido");
    }
}

