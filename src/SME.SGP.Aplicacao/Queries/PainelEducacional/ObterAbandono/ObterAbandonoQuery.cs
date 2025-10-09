using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono
{
    public class ObterAbandonoVisaoSmeDreQuery : IRequest<IEnumerable<PainelEducacionalAbandonoSmeDreDto>>
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public ObterAbandonoVisaoSmeDreQuery(int anoLetivo, string codigoDre)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
        }
    }
}
