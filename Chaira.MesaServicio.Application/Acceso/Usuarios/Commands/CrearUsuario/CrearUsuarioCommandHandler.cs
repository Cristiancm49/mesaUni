using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Mappers;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.CrearUsuario;

public sealed class CrearUsuarioCommandHandler : IRequestHandler<CrearUsuarioCommand, ApiResponseDto<UsuarioDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IGenericRepository<Rol> _rolRepository;

    public CrearUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IGenericRepository<Rol> rolRepository)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
    }

    public async Task<ApiResponseDto<UsuarioDto>> Handle(CrearUsuarioCommand request, CancellationToken cancellationToken)
    {
        var rolExiste = await _rolRepository.ExistsAsync(rol => rol.Id == request.Usuario.IdRol);
        if (!rolExiste)
        {
            return ApiResponseDto<UsuarioDto>.FailResponse("El rol seleccionado no existe.");
        }

        var entity = UsuarioMapper.ToEntity(request.Usuario);
        var created = await _usuarioRepository.AddAsync(entity);
        var entityWithRol = await _usuarioRepository.GetByIdWithRolAsync(created.Id);

        return ApiResponseDto<UsuarioDto>.SuccessResponse(UsuarioMapper.ToDto(entityWithRol!), "Usuario creado exitosamente");
    }
}

