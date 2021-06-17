using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaPAEEEncaminhamentoAEECommandValidator : AbstractValidator<GerarPendenciaPAEEEncaminhamentoAEECommand>
    {
        public GerarPendenciaPAEEEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEE)
                   .NotEmpty()
                   .WithMessage("O encaminhamento precisa ser informado.");
        }
    }
}
