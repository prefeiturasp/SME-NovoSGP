using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommand : IRequest<bool>
    {
        public ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommand(long questaoEncaminhamentoNAAPAId)
        {
            QuestaoEncaminhamentoNAAPAId = questaoEncaminhamentoNAAPAId;
        }

        public long QuestaoEncaminhamentoNAAPAId { get; }
    }

    public class ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommandValidator : AbstractValidator<ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommand>
    {
        public ExcluirRespostaEncaminhamentoNAAPAPorQuestaoIdCommandValidator()
        {
            RuleFor(c => c.QuestaoEncaminhamentoNAAPAId)
            .NotEmpty()
            .WithMessage("O id da questão do encaminhamento deve ser informado para exclusão de suas respostas.");
        }
    }
}
