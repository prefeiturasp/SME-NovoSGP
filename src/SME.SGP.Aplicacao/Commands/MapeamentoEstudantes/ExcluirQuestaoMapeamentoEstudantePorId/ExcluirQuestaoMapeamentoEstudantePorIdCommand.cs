using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoMapeamentoEstudantePorIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoMapeamentoEstudantePorIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class ExcluirQuestaoMapeamentoEstudantePorIdCommandValidator : AbstractValidator<ExcluirQuestaoMapeamentoEstudantePorIdCommand>
    {
        public ExcluirQuestaoMapeamentoEstudantePorIdCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .NotEmpty()
            .WithMessage("O id da questão deve ser informado para exclusão no mapeamento de estudante.");
        }
    }
}
