using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase : IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase
    {
        private readonly IMediator mediator;

        public ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>> Executar(FiltroGraficoFrequenciaTurmaEvasaoAlunoDto filtro)
        {
            return await mediator.Send(new ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery(filtro.AnoLetivo,
                filtro.DreCodigo, filtro.UeCodigo, filtro.TurmaCodigo, filtro.Modalidade, filtro.Semestre, filtro.Mes));
        }
    }
}
