using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdebPorAnoSerie
{
    public class ObterIdebPorAnoSerieQuery : IRequest<IEnumerable<PainelEducacionalIdebDto>>
    {
        public ObterIdebPorAnoSerieQuery(int anoLetivo, int serie, string codigoDre, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            Serie = serie;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }
        public int AnoLetivo { get; set; }
        public int Serie { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}
