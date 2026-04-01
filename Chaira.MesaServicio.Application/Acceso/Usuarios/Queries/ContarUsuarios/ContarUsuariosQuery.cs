using MediatR;
using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ContarUsuarios;

public sealed record ContarUsuariosQuery() : IRequest<ApiResponseDto<int>>;

