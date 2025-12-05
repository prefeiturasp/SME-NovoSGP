using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommand : IRequest<bool>
    {
        public ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommand(long questaoEncaminhamentoNAAPAId)
        {
            QuestaoEncaminhamentoNAAPAId = questaoEncaminhamentoNAAPAId;
        }

        public long QuestaoEncaminhamentoNAAPAId { get; }
    }

    public class ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommandValidator : AbstractValidator<ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommand>
    {
        public ExcluirRespostaAtendimentoNAAPAPorQuestaoIdCommandValidator()
        {
            RuleFor(c => c.QuestaoEncaminhamentoNAAPAId)
            .NotEmpty()
            .WithMessage("O id da questão do atendimento deve ser informado para exclusão de suas respostas.");
        }
    }
}
