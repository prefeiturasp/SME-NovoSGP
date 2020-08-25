using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarObservacaoDiarioBordoUseCase : IListarObservacaoDiarioBordoUseCase
    {
        private readonly IMediator mediator;

        public ListarObservacaoDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ListarObservacaoDiarioBordoDto>> Executar(long diarioBordoId)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new ListarObservacaoDiarioBordoQuery(diarioBordoId, usuarioId));
        }
    }
}
