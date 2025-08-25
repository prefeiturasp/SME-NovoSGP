using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaGlobal
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery : IRequest<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>>
    {
    }
}
