using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarios;

public sealed record ObtenerUsuariosQuery() : IRequest<ApiResponseDto<IEnumerable<UsuarioDto>>>;

