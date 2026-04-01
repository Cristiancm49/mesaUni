using FluentValidation;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorEmail;

public sealed class ObtenerUsuarioPorEmailQueryValidator : AbstractValidator<ObtenerUsuarioPorEmailQuery>
{
    public ObtenerUsuarioPorEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El formato del email no es valido.");
    }
}

