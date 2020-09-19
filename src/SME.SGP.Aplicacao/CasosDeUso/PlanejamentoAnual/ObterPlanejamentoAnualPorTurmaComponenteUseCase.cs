using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PlanejamentoAnual
{
    public class ObterPlanejamentoAnualPorTurmaComponenteUseCase
    {
        private readonly IMediator mediator;

        public ObterPlanejamentoAnualPorTurmaComponenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task Executar(long turmaId, long componenteCurricularId)
        {
            return await mediator.Send(new ObterPlanejamentoAnualPorTurmaComponenteQuery(turmaId, componenteCurricularId));
        }
    }
}
