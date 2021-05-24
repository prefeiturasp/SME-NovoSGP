using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardInformacoesEscolaresPorTurmaUseCase : IObterDashboardInformacoesEscolaresPorTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterDashboardInformacoesEscolaresPorTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroGraficoMatriculaDto filtro)
        {
            return await mediator.Send(new ObterDadosDashboardTurmaQuery(filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.Anos, filtro.Modalidade, filtro.Semestre));
        }
    }
}
