using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaMensal
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoMensalQuery : IRequest<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>>
    {
        public PainelEducacionalRegistroFrequenciaAgrupamentoMensalQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}
