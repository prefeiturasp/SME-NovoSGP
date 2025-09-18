using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQuery : IRequest<IEnumerable<ConsolidacaoInformacoesPap>>
    {
        public ObterIndicadoresPapQuery(string codigoDre = null, string codigoUe = null)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}