using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AdicionarObservacaoDiarioBordoUseCase : IAdicionarObservacaoDiarioBordoUseCase
    {
        private readonly IMediator mediator;

        public AdicionarObservacaoDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Executar(string observacao, long diarioBordoId, IEnumerable<long> usuarioIdsNotificacao)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new AdicionarObservacaoDiarioBordoCommand(diarioBordoId, observacao, usuarioId, usuarioIdsNotificacao));
        }
    }
}
