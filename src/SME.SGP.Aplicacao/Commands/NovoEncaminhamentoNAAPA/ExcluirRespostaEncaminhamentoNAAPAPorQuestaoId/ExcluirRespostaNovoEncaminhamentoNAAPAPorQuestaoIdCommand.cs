using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirRespostaEncaminhamentoNAAPAPorQuestaoId
{
    public class ExcluirRespostaNovoEncaminhamentoNAAPAPorQuestaoIdCommand : IRequest<bool>
    {
        public ExcluirRespostaNovoEncaminhamentoNAAPAPorQuestaoIdCommand(long questaoNovoEncaminhamentoNAAPAId)
        {
            QuestaoNovoEncaminhamentoNAAPAId = questaoNovoEncaminhamentoNAAPAId;
        }

        public long QuestaoNovoEncaminhamentoNAAPAId { get; }
    }
}