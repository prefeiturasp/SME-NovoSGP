using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecaoAtendimentoIndividualQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecaoAtendimentoIndividualQuery(long? encaminhamentoNAAPAId, long? tipoQuestionario = null)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
            TipoQuestionario = tipoQuestionario;
        }

        public long? EncaminhamentoNAAPAId { get; }
        public long? TipoQuestionario { get; }
    }
}

