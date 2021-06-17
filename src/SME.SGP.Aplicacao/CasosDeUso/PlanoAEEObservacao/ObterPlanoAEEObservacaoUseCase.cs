using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEObservacaoUseCase : AbstractUseCase, IObterPlanoAEEObservacaoUseCase
    {
        public ObterPlanoAEEObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<PlanoAEEObservacaoDto>> Executar(long planoAEEId)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            return await mediator.Send(new ObterObservacoesPlanoAEEPorIdQuery(planoAEEId, usuario.CodigoRf));
        }
    }
}
