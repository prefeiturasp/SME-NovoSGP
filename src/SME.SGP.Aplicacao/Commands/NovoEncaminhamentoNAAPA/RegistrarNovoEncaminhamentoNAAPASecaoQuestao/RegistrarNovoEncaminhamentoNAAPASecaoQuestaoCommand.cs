using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarNovoEncaminhamentoNAAPASecaoQuestao
{
    public class RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand : IRequest<long>
    {
        public long SecaoId { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand(long secaoId, long questaoId)
        {
            SecaoId = secaoId;
            QuestaoId = questaoId;
        }
    }

    public class RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommandValidator : AbstractValidator<RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommand>
    {
        public RegistrarNovoEncaminhamentoNAAPASecaoQuestaoCommandValidator()
        {
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Seção do Encaminhamento deve ser informada para registrar encaminhamento naapa!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Encaminhamento deve ser informada para registrar encaminhamento naapa!");
        }
    }
}