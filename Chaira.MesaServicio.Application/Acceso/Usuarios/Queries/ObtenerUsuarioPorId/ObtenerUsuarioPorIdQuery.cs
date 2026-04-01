using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorId;

public sealed record ObtenerUsuarioPorIdQuery(long Id) : IRequest<ApiResponseDto<UsuarioDto>>;

