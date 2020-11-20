using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosPorCodigoUeModalidadeQuery : IRequest<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>>
    {
        public string CodigoUe { get; set; }

        public Modalidade Modalidade { get; set; }

        public ObterAnosPorCodigoUeModalidadeQuery(string codigoUe, Modalidade modalidade)
        {
            CodigoUe = codigoUe;
            Modalidade = modalidade;
        }
    }
}
