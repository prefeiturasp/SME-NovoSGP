using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoEncaminhamentoAEEPorIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoEncaminhamentoAEEPorIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class ExcluirQuestaoEncaminhamentoAEEPorIdCommandValidator : AbstractValidator<ExcluirQuestaoEncaminhamentoAEEPorIdCommand>
    {
        public ExcluirQuestaoEncaminhamentoAEEPorIdCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .NotEmpty()
            .WithMessage("O id da questão deve ser informado para exclusão.");
        }
    }
}
