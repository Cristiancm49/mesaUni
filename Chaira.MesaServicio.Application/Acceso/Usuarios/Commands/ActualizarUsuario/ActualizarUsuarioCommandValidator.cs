using FluentValidation;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.ActualizarUsuario;

public sealed class ActualizarUsuarioCommandValidator : AbstractValidator<ActualizarUsuarioCommand>
{
    public ActualizarUsuarioCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El id del usuario debe ser mayor a cero.");

        RuleFor(x => x.Usuario.NombreCompleto)
            .MaximumLength(150).When(x => !string.IsNullOrWhiteSpace(x.Usuario.NombreCompleto))
            .WithMessage("El nombre no puede exceder 150 caracteres.");

        RuleFor(x => x.Usuario.Email)
            .MaximumLength(150).When(x => !string.IsNullOrWhiteSpace(x.Usuario.Email))
            .WithMessage("El email no puede exceder 150 caracteres.");

        RuleFor(x => x.Usuario.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Usuario.Email))
            .WithMessage("El formato del email no es valido.");

        RuleFor(x => x.Usuario.Telefono)
            .MaximumLength(20).When(x => x.Usuario.Telefono != null)
            .WithMessage("El telefono no puede exceder 20 caracteres.");

        RuleFor(x => x.Usuario.IdRol!.Value)
            .GreaterThan(0).When(x => x.Usuario.IdRol.HasValue)
            .WithMessage("El ID del rol debe ser mayor a cero.");
    }
}

