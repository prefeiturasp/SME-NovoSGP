using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasParaCopiaUseCase : IObterTurmasParaCopiaUseCase
    {
        private readonly IMediator mediator;

        public ObterTurmasParaCopiaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Executar(int turmaId, long componenteCurricularId, bool ensinoEspecial)
        {
            return await mediator.Send(new ObterTurmasEOLParaCopiaPorIdEComponenteCurricularIdQuery(turmaId, componenteCurricularId));
        }
    }
}
