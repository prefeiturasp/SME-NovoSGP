using MediatR;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAnoMaisRecenteIdeb
{
    public class ObterAnoMaisRecenteIdebQuery : IRequest<int?>
    {
        public string Serie { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }

        public ObterAnoMaisRecenteIdebQuery(string serie, string codigoDre, string codigoUe)
        {
            Serie = serie;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }
    }
}