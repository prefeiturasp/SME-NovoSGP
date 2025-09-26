using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterTaxaAlfabetizacao
{
    public class ObterTaxaAlfabetizacaoPorAnoLetivoQuery : IRequest<IEnumerable<TaxaAlfabetizacaoDto>>
    {
        public ObterTaxaAlfabetizacaoPorAnoLetivoQuery(int anoLetivo, string codigoDre, string codigoUe)
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
