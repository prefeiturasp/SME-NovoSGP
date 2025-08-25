using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaGlobal;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasRegistroFrequenciaAgrupamentoGlobalUseCase : IConsultasRegistroFrequenciaAgrupamentoGlobalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasRegistroFrequenciaAgrupamentoGlobalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoGlobalDto>> ObterFrequencia(string codigoDre, string codigoUe)
        {
            return await mediator.Send(new PainelEducacionalRegistroFrequenciaAgrupamentoGlobalQuery(codigoDre, codigoUe));
        }
    }
}
