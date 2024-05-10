using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterDREIdPorCodigoQueryValidator : AbstractValidator<ObterDREIdPorCodigoQuery>
    {
        public ObterDREIdPorCodigoQueryValidator()
        {
            RuleFor(a => a.CodigoDre)
                .NotEmpty()
                .WithMessage("O Código da Dre deve ser informado.");
        }
    }
}
