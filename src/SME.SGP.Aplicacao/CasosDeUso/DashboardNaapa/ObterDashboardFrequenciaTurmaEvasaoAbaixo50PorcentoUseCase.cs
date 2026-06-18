using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase : IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase
    {
        private readonly IMediator mediator;

        public ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FrequenciaTurmaEvasaoDto> Executar(FiltroGraficoFrequenciaTurmaEvasaoDto filtro)
        {
            return await mediator.Send(new ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery(filtro.AnoLetivo,
                filtro.DreCodigo, filtro.UeCodigo, filtro.Modalidade, filtro.Semestre, filtro.Mes));
        }
    }
}
