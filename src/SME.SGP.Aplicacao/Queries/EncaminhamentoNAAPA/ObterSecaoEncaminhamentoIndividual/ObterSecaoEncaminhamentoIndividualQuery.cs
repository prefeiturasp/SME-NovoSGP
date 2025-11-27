using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecaoEncaminhamentoIndividualQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecaoEncaminhamentoIndividualQuery(long? encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long? EncaminhamentoNAAPAId { get; }
    }
}

