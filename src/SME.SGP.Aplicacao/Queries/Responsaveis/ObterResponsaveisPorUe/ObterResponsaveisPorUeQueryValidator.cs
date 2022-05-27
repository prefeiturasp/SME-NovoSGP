using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorUeQueryValidator : AbstractValidator<ObterResponsaveisPorUeQuery>
    {
        public ObterResponsaveisPorUeQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");
        }
    }
}
