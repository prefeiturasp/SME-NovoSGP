using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand : IRequest<bool>
    {
        public ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand(long questaoEncaminhamentoAEEId)
        {
            QuestaoEncaminhamentoAEEId = questaoEncaminhamentoAEEId;
        }

        public long QuestaoEncaminhamentoAEEId { get; }
    }

    public class ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommandValidator : AbstractValidator<ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand>
    {
        public ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommandValidator()
        {
            RuleFor(c => c.QuestaoEncaminhamentoAEEId)
            .NotEmpty()
            .WithMessage("O id da questão do encaminhamento deve ser informado para exclusão de suas respostas.");
        }
    }
}
