using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEQueryValidator : AbstractValidator<ObterPlanosAEEQuery>
    {
        public ObterPlanosAEEQueryValidator()
        {
            RuleFor(c => c.DreId)
            .NotEmpty()
            .WithMessage("A DRE deve ser informada para pesquisa de Planos AEE");
        }
    }
}
