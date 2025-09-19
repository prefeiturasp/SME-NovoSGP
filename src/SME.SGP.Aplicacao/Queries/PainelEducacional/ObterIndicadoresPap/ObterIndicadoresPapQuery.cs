using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQuery : IRequest<IEnumerable<PainelEducacionalInformacoesPapDto>>
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