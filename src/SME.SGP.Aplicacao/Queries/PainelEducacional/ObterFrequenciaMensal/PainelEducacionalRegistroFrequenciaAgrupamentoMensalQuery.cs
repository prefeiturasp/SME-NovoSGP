using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaMensal
{
    public class PainelEducacionalRegistroFrequenciaAgrupamentoMensalQuery : IRequest<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>>
    {
    }
}
