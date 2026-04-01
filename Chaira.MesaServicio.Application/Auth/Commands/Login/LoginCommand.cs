using MediatR;
using Chaira.MesaServicio.Application.Auth.Models;

namespace Chaira.MesaServicio.Application.Auth.Commands.Login;

public sealed record LoginCommand(LoginDto Credentials) : IRequest<LoginResponseDto>;

