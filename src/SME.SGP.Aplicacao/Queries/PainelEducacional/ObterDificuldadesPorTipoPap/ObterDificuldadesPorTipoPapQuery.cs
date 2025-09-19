using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesPorTipoPap
{
    public class ObterDificuldadesPorTipoPapQuery : IRequest<ContagemDificuldadePorTipoDto>
    {
        public ObterDificuldadesPorTipoPapQuery(TipoPap tipoPap, string codigoDre, string codigoUe)
        {
            TipoPap = tipoPap;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public TipoPap TipoPap { get; }
        public string CodigoDre { get; }
        public string CodigoUe { get; set; }
    }
}