using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaMensal;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasRegistroFrequenciaAgrupamentoMensalUseCase : IConsultasRegistroFrequenciaAgrupamentoMensalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasRegistroFrequenciaAgrupamentoMensalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensalDto>> ObterFrequencia(string codigoDre, string codigoUe)
        {
            return await mediator.Send(new PainelEducacionalRegistroFrequenciaAgrupamentoMensalQuery(codigoDre, codigoUe));
        }
    }
}
