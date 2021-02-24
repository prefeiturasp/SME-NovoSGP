using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncaminhamentoAEECommandValidator : AbstractValidator<GerarPendenciaCPEncaminhamentoAEECommand>
    {
        public GerarPendenciaCPEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEEId)
                   .NotEmpty()
                   .WithMessage("O encaminhamento ID precisa ser informado.");

            RuleFor(c => c.Situacao)
                    .NotEmpty()
                    .WithMessage("O situação do EncaminhamentoAEE precisa ser informada.");
        }
    }
}
