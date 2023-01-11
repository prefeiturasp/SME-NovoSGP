using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarQuestaoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public AlterarQuestaoEncaminhamentoNAAPACommand(QuestaoEncaminhamentoNAAPA questao)
        {
            Questao = questao;
        }

        public QuestaoEncaminhamentoNAAPA Questao { get; }
    }

    public class AlterarQuestaoEncaminhamentoNAAPACommandValidator : AbstractValidator<AlterarQuestaoEncaminhamentoNAAPACommand>
    {
        public AlterarQuestaoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.Questao)
            .NotEmpty()
            .WithMessage("A questão do encaminhamento NAAPA deve ser informada para alteração.");
        }
    }
}
