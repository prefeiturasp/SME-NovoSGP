using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao.Commands.EncaminhamentoNAAPA.ExcluirQuestaoNovoEncaminhamentoNAAPAPorId
{
    public class ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand>
    {
        public ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .NotEmpty()
            .WithMessage("O id da questão deve ser informado para exclusão no encaminhamento naapa.");
        }
    }
}