using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoRegistroAcaoPorIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoRegistroAcaoPorIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class ExcluirQuestaoRegistroAcaoPorIdCommandValidator : AbstractValidator<ExcluirQuestaoRegistroAcaoPorIdCommand>
    {
        public ExcluirQuestaoRegistroAcaoPorIdCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .NotEmpty()
            .WithMessage("O id da questão deve ser informado para exclusão no registro de ação.");
        }
    }
}
