using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaGlobal
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery : IRequest<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>>
    {
        public PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery(int anoLetivo, string codigoDre, string codigoUe)
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
