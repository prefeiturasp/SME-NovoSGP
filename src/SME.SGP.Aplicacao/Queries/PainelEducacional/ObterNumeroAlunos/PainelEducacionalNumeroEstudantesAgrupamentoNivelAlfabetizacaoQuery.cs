using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos
{
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery : IRequest<IEnumerable<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>>
    {
        public PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(string anoLetivo, string periodo)
        {
            AnoLetivo = anoLetivo;
            Periodo = periodo;
        }

        public string AnoLetivo { get; set; }
        public string Periodo { get; set; }
    }
}
