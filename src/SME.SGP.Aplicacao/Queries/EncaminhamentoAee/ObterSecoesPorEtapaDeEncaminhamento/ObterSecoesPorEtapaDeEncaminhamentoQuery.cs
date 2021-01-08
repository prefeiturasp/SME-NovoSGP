using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public long Etapa { get; set; }

        public ObterSecoesPorEtapaDeEncaminhamentoQuery(long etapa)
        {
            Etapa = etapa;
        }
    }

}
