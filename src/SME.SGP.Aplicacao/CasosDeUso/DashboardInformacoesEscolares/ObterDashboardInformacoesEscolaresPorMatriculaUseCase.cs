using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardInformacoesEscolaresPorMatriculaUseCase : IObterDashboardInformacoesEscolaresPorMatriculaUseCase
    {
        private readonly IMediator mediator;

        public ObterDashboardInformacoesEscolaresPorMatriculaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroGraficoMatriculaDto filtro)
        {
            return await mediator.Send(new ObterDadosDashboardMatriculaQuery(filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.Anos, filtro.Modalidade, filtro.Semestre));
        }
    }
}
