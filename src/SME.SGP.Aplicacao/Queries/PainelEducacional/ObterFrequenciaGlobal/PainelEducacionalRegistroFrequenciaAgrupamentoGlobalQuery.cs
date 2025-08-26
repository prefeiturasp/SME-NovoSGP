using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaGlobal
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery : IRequest<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>>
    {
        public PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
    }
}
