using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaRanking
{
    public class PainelEducacionalRegistroFrequenciaRakingQuery : IRequest<PainelEducacionalRegistroFrequenciaRankingDto>
    {
        public PainelEducacionalRegistroFrequenciaRakingQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}
