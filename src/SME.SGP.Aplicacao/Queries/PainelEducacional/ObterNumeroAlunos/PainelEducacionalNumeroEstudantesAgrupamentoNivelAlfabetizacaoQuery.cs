using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos
{
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery : IRequest<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>>
    {
        public PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(int anoLetivo, int periodo, string codigoDre = null, string codigoUe = null)
        {
            AnoLetivo = anoLetivo;
            Periodo = periodo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public int Periodo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}
