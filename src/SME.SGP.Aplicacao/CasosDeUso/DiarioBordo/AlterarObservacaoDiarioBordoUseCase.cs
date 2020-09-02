using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarObservacaoDiarioBordoUseCase : IAlterarObservacaoDiarioBordoUseCase
    {
        private readonly IMediator mediator;

        public AlterarObservacaoDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Executar(string observacao, long observacaoId)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new AlterarObservacaoDiarioBordoCommand(observacao, observacaoId, usuarioId));
        }
    }
}
