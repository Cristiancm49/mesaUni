using MediatR;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ContarUsuarios;

public sealed class ContarUsuariosQueryHandler : IRequestHandler<ContarUsuariosQuery, ApiResponseDto<int>>
{
    private readonly IGenericRepository<Usuario> _usuarioRepository;

    public ContarUsuariosQueryHandler(IGenericRepository<Usuario> usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ApiResponseDto<int>> Handle(ContarUsuariosQuery request, CancellationToken cancellationToken)
    {
        var total = await _usuarioRepository.CountAsync();
        return ApiResponseDto<int>.SuccessResponse(total, "Conteo obtenido");
    }
}

