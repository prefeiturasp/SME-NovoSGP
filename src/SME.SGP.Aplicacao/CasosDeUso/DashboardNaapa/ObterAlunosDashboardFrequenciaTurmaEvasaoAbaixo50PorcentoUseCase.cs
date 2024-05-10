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
            var filtroAbrangencia = new FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto()
            {
                AnoLetivo = filtro.AnoLetivo,
                DreCodigo = filtro.DreCodigo,
                UeCodigo = filtro.UeCodigo,
                TurmaCodigo = filtro.TurmaCodigo,
                Modalidade = filtro.Modalidade,
                Semestre = filtro.Semestre
            };
            return await mediator.Send(new ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoQuery(filtroAbrangencia, filtro.Mes));
        }
    }
}
