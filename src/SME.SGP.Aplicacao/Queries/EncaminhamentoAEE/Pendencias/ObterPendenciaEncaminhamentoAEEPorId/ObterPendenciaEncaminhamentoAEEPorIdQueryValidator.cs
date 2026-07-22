using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaEncaminhamentoAEEPorIdQueryValidator : AbstractValidator<ObterPendenciaEncaminhamentoAEEPorIdQuery>
    {
        public ObterPendenciaEncaminhamentoAEEPorIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
               .NotEmpty()
               .WithMessage("O Id encaminhamento deve ser informado.");
        }
    }
}
