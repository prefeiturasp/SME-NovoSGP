using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaPorAnoUseCase : IObterDashboardFrequenciaPorAnoUseCase
    {
        private readonly IMediator mediator;

        public ObterDashboardFrequenciaPorAnoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoFrequenciaGlobalPorAnoDto>> Executar(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre)
        {
            return await mediator.Send(new ObterDadosDashboardFrequenciaPorAnoQuery(anoLetivo, dreId, ueId, modalidade, semestre));
        }
    }
}
