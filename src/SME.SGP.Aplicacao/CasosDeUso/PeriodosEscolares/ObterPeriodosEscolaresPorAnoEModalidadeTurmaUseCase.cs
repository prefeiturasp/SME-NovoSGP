using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase : IObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterPeriodosEscolaresPorAnoEModalidadeTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PeriodoEscolarDto>> Executar(Modalidade modalidade, int anoLetivo)
        {
            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(modalidade, anoLetivo));
            return periodos?.Select(c => new PeriodoEscolarDto
            {
                Bimestre = c.Bimestre,
                Id = c.Id,
                Migrado = c.Migrado,
                PeriodoFim = c.PeriodoFim,
                PeriodoInicio = c.PeriodoInicio
            });
        }
    }
}
