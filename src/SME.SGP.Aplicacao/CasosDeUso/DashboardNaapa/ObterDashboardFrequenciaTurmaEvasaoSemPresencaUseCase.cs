using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
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

        public async Task<FrequenciaTurmaEvasaoDto> Executar(FiltroGraficoFrequenciaTurmaEvasaoDto filtro)
        {
            return await mediator.Send(new ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery(filtro.AnoLetivo, 
                filtro.DreCodigo, filtro.UeCodigo, filtro.Modalidade, filtro.Semestre, filtro.Mes));
        }
    }
}
