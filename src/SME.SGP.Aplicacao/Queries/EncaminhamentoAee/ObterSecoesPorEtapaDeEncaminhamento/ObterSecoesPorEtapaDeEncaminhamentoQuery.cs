using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoQuery(List<int> etapas, long encaminhamentoAeeId)
        {
            Etapas = etapas;
            EncaminhamentoAeeId = encaminhamentoAeeId;
        }

        public List<int> Etapas { get; set; }
        public long EncaminhamentoAeeId { get; }

    }

}
