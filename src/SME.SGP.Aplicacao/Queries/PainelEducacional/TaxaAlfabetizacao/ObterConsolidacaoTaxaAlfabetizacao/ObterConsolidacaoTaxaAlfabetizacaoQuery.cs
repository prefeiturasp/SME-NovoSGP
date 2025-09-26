using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterConsolidacaoTaxaAlfabetizacao
{
    public class ObterConsolidacaoTaxaAlfabetizacaoQuery : IRequest<IEnumerable<PainelEducacionalConsolidacaoTaxaAlfabetizacao>>
    {
        public ObterConsolidacaoTaxaAlfabetizacaoQuery(int anoLetivo, string codigoDre, string codigoUe)
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
