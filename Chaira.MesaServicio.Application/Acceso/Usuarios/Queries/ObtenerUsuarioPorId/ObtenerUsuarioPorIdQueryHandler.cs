using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Mappers;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorId;

public sealed class ObtenerUsuarioPorIdQueryHandler : IRequestHandler<ObtenerUsuarioPorIdQuery, ApiResponseDto<UsuarioDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ObtenerUsuarioPorIdQueryHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ApiResponseDto<UsuarioDto>> Handle(ObtenerUsuarioPorIdQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdWithRolAsync(request.Id);

        if (usuario is null)
        {
            return ApiResponseDto<UsuarioDto>.FailResponse("Usuario no encontrado");
        }

        return ApiResponseDto<UsuarioDto>.SuccessResponse(UsuarioMapper.ToDto(usuario), "Usuario obtenido");
    }
}

