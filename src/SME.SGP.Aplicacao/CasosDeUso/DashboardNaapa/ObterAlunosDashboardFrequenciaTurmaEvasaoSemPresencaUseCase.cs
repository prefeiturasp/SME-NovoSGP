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
            return await mediator.Send(new ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaQuery(filtro.AnoLetivo,
                filtro.DreCodigo, filtro.UeCodigo, filtro.TurmaCodigo, filtro.Modalidade, filtro.Semestre, filtro.Mes));
        }
    }
}
