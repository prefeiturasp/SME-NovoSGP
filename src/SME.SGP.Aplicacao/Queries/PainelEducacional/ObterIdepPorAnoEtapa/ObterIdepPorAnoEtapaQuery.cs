using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa
{
    public class ObterIdepPorAnoEtapaQuery : IRequest<IEnumerable<PainelEducacionalIdepDto>>
    {
        public ObterIdepPorAnoEtapaQuery(int anoLetivo, string etapa, string codigoDre)
        {
            AnoLetivo = anoLetivo;
            Etapa = etapa;
            CodigoDre = codigoDre;
        }
        public int AnoLetivo { get; set; }
        public string Etapa { get; set; }
        public string CodigoDre { get; set; }
    }
}
