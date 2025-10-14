using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterConsolidacaoSondagemEscrita
{
    public class ObterConsolidacaoSondagemEscritaUeQuery : IRequest<IEnumerable<PainelEducacionalConsolidacaoSondagemEscritaUe>>
    {
        public ObterConsolidacaoSondagemEscritaUeQuery(int anoLetivo, int bimestre, string codigoUe, string codigoDre)
        {
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            CodigoUe = codigoUe;
            CodigoDre = codigoDre;
        }

        public int AnoLetivo { get; set; }
        public int Bimestre { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoDre { get; set; }
    }
}
