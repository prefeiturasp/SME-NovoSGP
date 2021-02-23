using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCEFAIEncaminhamentoAEECommandValidator : AbstractValidator<GerarPendenciaCEFAIEncaminhamentoAEECommand>
    {
        public GerarPendenciaCEFAIEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEE)
                   .NotEmpty()
                   .WithMessage("O encaminhamento precisa ser informado.");
        }
    }
}
