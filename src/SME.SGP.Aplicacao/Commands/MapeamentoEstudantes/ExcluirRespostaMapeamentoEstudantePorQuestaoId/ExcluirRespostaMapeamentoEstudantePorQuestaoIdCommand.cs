using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommand : IRequest<bool>
    {
        public ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommand(long questaoMapeamentoEstudanteId)
        {
            QuestaoMapeamentoEstudanteId = questaoMapeamentoEstudanteId;
        }

        public long QuestaoMapeamentoEstudanteId { get; }
    }

    public class ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommandValidator : AbstractValidator<ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommand>
    {
        public ExcluirRespostaMapeamentoEstudantePorQuestaoIdCommandValidator()
        {
            RuleFor(c => c.QuestaoMapeamentoEstudanteId)
            .NotEmpty()
            .WithMessage("O id da questão do mapeamento deve ser informado para exclusão de suas respostas.");
        }
    }
}
