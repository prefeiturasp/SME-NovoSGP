using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosPorModalidadeECodigoUeQueryValidator : AbstractValidator<ObterCiclosPorModalidadeECodigoUeQuery>
    {
        public ObterCiclosPorModalidadeECodigoUeQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado.");

            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada.");
        }
    }
}