using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoAtendimentoNAAPACommand : IRequest<bool>
    {
        public AlterarQuestaoAtendimentoNAAPACommand(QuestaoEncaminhamentoNAAPA questao)
        {
            Questao = questao;
        }

        public QuestaoEncaminhamentoNAAPA Questao { get; }
    }

    public class AlterarQuestaoAtendimentoNAAPACommandValidator : AbstractValidator<AlterarQuestaoAtendimentoNAAPACommand>
    {
        public AlterarQuestaoAtendimentoNAAPACommandValidator()
        {
            RuleFor(c => c.Questao)
            .NotEmpty()
            .WithMessage("A questão do atendimento NAAPA deve ser informada para alteração.");
        }
    }
}
