using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpQuery : IRequest<IEnumerable<ContagemDificuldadePorTipoDto>>
    {
        public ObterIndicadoresPapSgpQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}