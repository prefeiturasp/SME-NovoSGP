using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoEncaminhamentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoEncaminhamentoNAAPAPorIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class ExcluirQuestaoEncaminhamentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirQuestaoEncaminhamentoNAAPAPorIdCommand>
    {
        public ExcluirQuestaoEncaminhamentoNAAPAPorIdCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .NotEmpty()
            .WithMessage("O id da questão deve ser informado para exclusão no encaminhamento naapa.");
        }
    }
}
