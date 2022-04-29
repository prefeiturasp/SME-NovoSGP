using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase : IObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase
    {
        private readonly IMediator _mediator;

        public ObterDashboardFrequenciaTurmaEvasaoSemPresencaUseCase(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> Executar(FiltroGraficoFrequenciaTurmaEvasaoDto filtro)
        {
            return await _mediator.Send(new ObterDashboardFrequenciaTurmaEvasaoSemPresencaQuery(filtro.DreCodigo, filtro.UeCodigo,
                filtro.Modalidade, filtro.Semestre, filtro.Mes));
        }
    }
}
