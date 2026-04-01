using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.ActualizarUsuario;

public sealed record ActualizarUsuarioCommand(long Id, UsuarioUpdateDto Usuario) : IRequest<ApiResponseDto<UsuarioDto>>;

