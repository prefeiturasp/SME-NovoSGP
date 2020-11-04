using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase : IObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase
    {
        private readonly IMediator mediator;

        public ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> Executar(long planejamentoAnualId)
        {
            return await mediator.Send(new ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery(planejamentoAnualId));
        }
    }
}
