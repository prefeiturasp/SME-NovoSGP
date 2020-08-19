using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoDiarioBordoUseCase : IExcluirObservacaoDiarioBordoUseCase
    {
        private readonly IMediator mediator;

        public ExcluirObservacaoDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long diarioBordoId)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new ExcluirObservacaoDiarioBordoCommand(diarioBordoId, usuarioId));
        }
    }
}
