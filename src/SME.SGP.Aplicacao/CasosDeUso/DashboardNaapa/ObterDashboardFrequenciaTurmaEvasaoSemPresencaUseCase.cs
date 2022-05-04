using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase : IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase
    {
        private readonly IMediator mediator;

        public ObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> Executar(FiltroGraficoFrequenciaTurmaEvasaoDto filtro)
        {
            return await mediator.Send(new ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery(filtro.AnoLetivo, 
                filtro.DreCodigo, filtro.UeCodigo, filtro.Modalidade, filtro.Semestre, filtro.Mes));
        }
    }
}
