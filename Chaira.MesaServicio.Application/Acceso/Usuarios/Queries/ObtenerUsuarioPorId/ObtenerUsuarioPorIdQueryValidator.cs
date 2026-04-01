using FluentValidation;

namespace Chaira.MesaServicio.Application.Acceso.Usuarios.Queries.ObtenerUsuarioPorId;

public sealed class ObtenerUsuarioPorIdQueryValidator : AbstractValidator<ObtenerUsuarioPorIdQuery>
{
    public ObtenerUsuarioPorIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El id del usuario debe ser mayor a cero.");
    }
}

