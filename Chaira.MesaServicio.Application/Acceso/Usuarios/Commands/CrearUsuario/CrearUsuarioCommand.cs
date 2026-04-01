using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.CrearUsuario;

public sealed record CrearUsuarioCommand(UsuarioCreateDto Usuario) : IRequest<ApiResponseDto<UsuarioDto>>;

