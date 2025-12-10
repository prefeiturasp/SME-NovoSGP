using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.AlterarQuestaoEncaminhamentoNAAPA
{
    public class AlterarQuestaoNovoEncaminhamentoNAAPACommand : IRequest<bool>
    {
        public AlterarQuestaoNovoEncaminhamentoNAAPACommand(QuestaoEncaminhamentoEscolar questao)
        {
            Questao = questao;
        }

        public QuestaoEncaminhamentoEscolar Questao { get; }
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
