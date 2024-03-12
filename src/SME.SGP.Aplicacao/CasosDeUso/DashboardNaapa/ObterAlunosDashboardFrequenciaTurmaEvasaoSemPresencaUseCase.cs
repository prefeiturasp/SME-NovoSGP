using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase : IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase
    {
        private readonly IMediator mediator;

        public ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase(IMediator mediator)
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
            return await mediator.Send(new ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery(filtroAbrangencia, filtro.Mes));
        }
    }
}
