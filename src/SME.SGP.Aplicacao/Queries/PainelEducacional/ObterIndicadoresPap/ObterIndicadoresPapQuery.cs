using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQuery : IRequest<IndicadoresPapDto>
    {
        public ObterIndicadoresPapQuery(int anoLetivo, string codigoDre, string codigoUe)
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