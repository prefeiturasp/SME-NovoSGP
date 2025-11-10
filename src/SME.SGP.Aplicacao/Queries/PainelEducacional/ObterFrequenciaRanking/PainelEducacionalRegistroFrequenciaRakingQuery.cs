using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaRanking
{
    public class PainelEducacionalRegistroFrequenciaRakingQuery : IRequest<PainelEducacionalRegistroFrequenciaRankingDto>
    {
        public PainelEducacionalRegistroFrequenciaRakingQuery(int anoLetivo, string codigoDre, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; } 
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}
