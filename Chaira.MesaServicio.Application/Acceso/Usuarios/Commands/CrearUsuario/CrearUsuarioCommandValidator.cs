using FluentValidation;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Commands.CrearUsuario;

public sealed class CrearUsuarioCommandValidator : AbstractValidator<CrearUsuarioCommand>
{
    public CrearUsuarioCommandValidator()
    {
        RuleFor(x => x.Usuario.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es requerido.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres.");

        RuleFor(x => x.Usuario.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .MaximumLength(150).WithMessage("El email no puede exceder 150 caracteres.")
            .EmailAddress().WithMessage("El formato del email no es valido.");

        RuleFor(x => x.Usuario.Telefono)
            .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Usuario.Telefono))
            .WithMessage("El telefono no puede exceder 20 caracteres.");

        RuleFor(x => x.Usuario.IdRol)
            .GreaterThan(0).WithMessage("El ID del rol es requerido.");

        RuleFor(x => x.Usuario.IdUsuarioCreacion)
            .GreaterThan(0).WithMessage("El ID del usuario de creacion es requerido.");
    }
}

