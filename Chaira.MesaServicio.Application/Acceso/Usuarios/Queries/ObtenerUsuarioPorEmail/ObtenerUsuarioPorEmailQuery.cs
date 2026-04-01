using MediatR;
using Chaira.MesaServicio.Application.Acceso.Usuarios.Models;
using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorEmail;

public sealed record ObtenerUsuarioPorEmailQuery(string Email) : IRequest<ApiResponseDto<UsuarioDto>>;

