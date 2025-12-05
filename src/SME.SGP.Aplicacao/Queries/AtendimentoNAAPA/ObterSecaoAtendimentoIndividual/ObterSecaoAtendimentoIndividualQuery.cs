using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecaoAtendimentoIndividualQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecaoAtendimentoIndividualQuery(long? encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long? EncaminhamentoNAAPAId { get; }
    }
}

