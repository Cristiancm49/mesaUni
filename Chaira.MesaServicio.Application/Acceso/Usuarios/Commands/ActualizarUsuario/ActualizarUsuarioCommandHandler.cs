using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Mappers;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Models.Acceso;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.ActualizarUsuario;

public sealed class ActualizarUsuarioCommandHandler : IRequestHandler<ActualizarUsuarioCommand, ApiResponseDto<UsuarioDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IGenericRepository<Rol> _rolRepository;

    public ActualizarUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IGenericRepository<Rol> rolRepository)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
    }

    public async Task<ApiResponseDto<UsuarioDto>> Handle(ActualizarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var entity = await _usuarioRepository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return ApiResponseDto<UsuarioDto>.FailResponse($"Usuario con ID {request.Id} no encontrado");
        }

        if (request.Usuario.IdRol.HasValue)
        {
            var rolExiste = await _rolRepository.ExistsAsync(rol => rol.Id == request.Usuario.IdRol.Value);
            if (!rolExiste)
            {
                return ApiResponseDto<UsuarioDto>.FailResponse("El rol seleccionado no existe.");
            }
        }

        UsuarioMapper.ApplyUpdate(entity, request.Usuario);
        await _usuarioRepository.UpdateAsync(entity);
        var entityWithRol = await _usuarioRepository.GetByIdWithRolAsync(request.Id);

        return ApiResponseDto<UsuarioDto>.SuccessResponse(UsuarioMapper.ToDto(entityWithRol!), "Usuario actualizado exitosamente");
    }
}

