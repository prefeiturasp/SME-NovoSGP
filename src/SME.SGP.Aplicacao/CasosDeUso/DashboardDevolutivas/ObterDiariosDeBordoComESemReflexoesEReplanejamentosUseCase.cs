using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoComESemReflexoesEReplanejamentosUseCase : IObterDiariosDeBordoComESemReflexoesEReplanejamentosUseCase
    {
        private readonly IMediator mediator;

        public ObterDiariosDeBordoComESemReflexoesEReplanejamentosUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto>> Executar(FiltroGraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto filtro)
            => await mediator.Send(new ObterDiariosDeBordoComESemReflexoesEReplanejamentosQuery(filtro.AnoLetivo, filtro.Modalidade, DateTime.Today, filtro.DreId, filtro.UeId));
    }
}
