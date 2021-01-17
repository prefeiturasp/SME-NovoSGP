using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoQuery(List<int> etapas)
        {
            Etapas = etapas;
        }

        public List<int> Etapas { get; set; }

    }

}
