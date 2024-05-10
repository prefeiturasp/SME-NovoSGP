using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaRegistroAcaoPorQuestaoIdCommand : IRequest<bool>
    {
        public ExcluirRespostaRegistroAcaoPorQuestaoIdCommand(long questaoRegistroAcaoId)
        {
            QuestaoRegistroAcaoId = questaoRegistroAcaoId;
        }

        public long QuestaoRegistroAcaoId { get; }
    }

    public class ExcluirRespostaRegistroAcaoPorQuestaoIdCommandValidator : AbstractValidator<ExcluirRespostaRegistroAcaoPorQuestaoIdCommand>
    {
        public ExcluirRespostaRegistroAcaoPorQuestaoIdCommandValidator()
        {
            RuleFor(c => c.QuestaoRegistroAcaoId)
            .NotEmpty()
            .WithMessage("O id da questão do encaminhamento deve ser informado para exclusão de suas respostas.");
        }
    }
}
