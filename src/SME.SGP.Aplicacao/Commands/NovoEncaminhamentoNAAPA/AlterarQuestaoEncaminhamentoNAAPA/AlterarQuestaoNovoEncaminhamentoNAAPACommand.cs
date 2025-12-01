using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarQuestaoEncaminhamentoNAAPA
{
    public class AlterarQuestaoNovoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public AlterarQuestaoNovoEncaminhamentoNAAPACommand(QuestaoEncaminhamentoNAAPA questao)
        {
            Questao = questao;
        }

        public QuestaoEncaminhamentoNAAPA Questao { get; }
    }

    public class AlterarQuestaoNovoEncaminhamentoNAAPACommandValidator : AbstractValidator<AlterarQuestaoNovoEncaminhamentoNAAPACommand>
    {
        public AlterarQuestaoNovoEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(c => c.Questao)
            .NotEmpty()
            .WithMessage("A questão do encaminhamento NAAPA deve ser informada para alteração.");
        }
    }
}
