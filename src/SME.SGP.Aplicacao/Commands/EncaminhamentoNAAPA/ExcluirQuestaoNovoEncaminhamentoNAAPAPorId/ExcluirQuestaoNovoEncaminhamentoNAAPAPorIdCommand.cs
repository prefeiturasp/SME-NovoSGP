using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.EncaminhamentoNAAPA.ExcluirQuestaoNovoEncaminhamentoNAAPAPorId
{
    public class ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }

    public class ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommandValidator : AbstractValidator<ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommand>
    {
        public ExcluirQuestaoNovoEncaminhamentoNAAPAPorIdCommandValidator()
        {
            RuleFor(c => c.QuestaoId)
            .NotEmpty()
            .WithMessage("O id da questão deve ser informado para exclusão no encaminhamento naapa.");
        }
    }
}