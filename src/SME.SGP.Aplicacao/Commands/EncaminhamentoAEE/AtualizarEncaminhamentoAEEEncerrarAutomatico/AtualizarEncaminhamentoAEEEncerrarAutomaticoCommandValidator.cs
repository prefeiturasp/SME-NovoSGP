using FluentValidation;

namespace SME.SGP.Aplicacao.Commands
{
    public class AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandValidator : AbstractValidator<AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand>
    {
        public AtualizarEncaminhamentoAEEEncerrarAutomaticoCommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                   .WithMessage("O ID do Encaminhamento é obrigatório!");
        }
    }
}
