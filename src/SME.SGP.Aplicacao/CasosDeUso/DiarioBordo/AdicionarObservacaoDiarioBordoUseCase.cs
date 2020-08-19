using MediatR;
using SME.SGP.Infra;
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

        public async Task<AuditoriaDto> Executar(string observacao, long diarioBordoId)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new AdicionarObservacaoCommand(diarioBordoId, observacao, usuarioId));
        }
    }
}
