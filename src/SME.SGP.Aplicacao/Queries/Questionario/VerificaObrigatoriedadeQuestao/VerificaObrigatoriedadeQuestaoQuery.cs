using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class VerificaObrigatoriedadeQuestaoQuery : IRequest<bool>
    {
        public VerificaObrigatoriedadeQuestaoQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class VerificaObrigatoriedadeQuestaoQueryValidator : AbstractValidator<VerificaObrigatoriedadeQuestaoQuery>
    {
        public VerificaObrigatoriedadeQuestaoQueryValidator()
        {
            RuleFor(a => a.QuestaoId)
                .NotEmpty()
                .WithMessage("O id da questão é obrigatório para consulta de sua obrigatoriedade");
        }
    }
}
