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

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> Executar(long turmaId, long componenteCurricularId, bool ensinoEspecial)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            return await mediator.Send(new ObterTurmasParaCopiaPlanejamentoAnualQuery(turmaId, componenteCurricularId, usuario.CodigoRf, ensinoEspecial));
        }
    }
}
