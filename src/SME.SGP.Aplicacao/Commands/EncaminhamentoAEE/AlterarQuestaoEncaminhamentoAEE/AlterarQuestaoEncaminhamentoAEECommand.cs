using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoEncaminhamentoAEECommand : IRequest<bool>
    {
        public AlterarQuestaoEncaminhamentoAEECommand(QuestaoEncaminhamentoAEE questao)
        {
            Questao = questao;
        }

        public QuestaoEncaminhamentoAEE Questao { get; }
    }

    public class AlterarQuestaoEncaminhamentoAEECommandValidator : AbstractValidator<AlterarQuestaoEncaminhamentoAEECommand>
    {
        public AlterarQuestaoEncaminhamentoAEECommandValidator()
        {
            RuleFor(c => c.Questao)
            .NotEmpty()
            .WithMessage("A questão do encaminhamento AEE deve ser informada para alteração.");
        }
    }
}
