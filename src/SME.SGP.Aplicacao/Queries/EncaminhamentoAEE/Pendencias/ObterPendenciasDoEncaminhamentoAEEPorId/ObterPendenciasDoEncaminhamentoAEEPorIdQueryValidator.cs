using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasDoEncaminhamentoAEEPorIdQueryValidator : AbstractValidator<ObterPendenciasDoEncaminhamentoAEEPorIdQuery>
    {
        public ObterPendenciasDoEncaminhamentoAEEPorIdQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
               .NotEmpty()
               .WithMessage("O Id encaminhamento deve ser informado.");
        }
    }
}
