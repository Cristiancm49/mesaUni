using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Mappers;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarios;

public sealed class ObtenerUsuariosQueryHandler : IRequestHandler<ObtenerUsuariosQuery, ApiResponseDto<IEnumerable<UsuarioDto>>>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ObtenerUsuariosQueryHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ApiResponseDto<IEnumerable<UsuarioDto>>> Handle(ObtenerUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioRepository.GetAllWithRolAsync();

        var data = usuarios.Select(UsuarioMapper.ToDto);

        return ApiResponseDto<IEnumerable<UsuarioDto>>.SuccessResponse(data, "Usuarios obtenidos");
    }
}

