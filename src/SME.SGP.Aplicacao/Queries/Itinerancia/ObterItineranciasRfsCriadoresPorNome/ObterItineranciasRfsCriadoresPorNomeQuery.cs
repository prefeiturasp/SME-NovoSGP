using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasRfsCriadoresPorNomeQuery : IRequest<IEnumerable<ItineranciaNomeRfCriadorRetornoDto>>
    {
        public ObterItineranciasRfsCriadoresPorNomeQuery(string nomeParaBusca)
        {
            NomeParaBusca = nomeParaBusca;
        }

        public string NomeParaBusca { get; set; }
    }
}
