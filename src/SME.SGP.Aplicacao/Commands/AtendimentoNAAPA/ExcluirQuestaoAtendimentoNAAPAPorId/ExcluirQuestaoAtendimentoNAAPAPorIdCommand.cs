using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoAtendimentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoAtendimentoNAAPAPorIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class ExcluirQuestaoAtendimentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirQuestaoAtendimentoNAAPAPorIdCommand>
    {
        public ExcluirQuestaoAtendimentoNAAPAPorIdCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .NotEmpty()
            .WithMessage("O id da questão deve ser informado para exclusão no atendimento naapa.");
        }
    }
}
