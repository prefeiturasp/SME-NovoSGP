using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacoesNiveisCargosUseCase : ITrataNotificacoesNiveisCargosUseCase
    {
        private readonly IMediator mediator;

        public TrataNotificacoesNiveisCargosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task Executar()
        {
            var listaDeNotificacoesParaTratar = await mediator.Send(new ObterNotificacoesNiveisCargosQuery());
            await mediator.Send(new TrataNotificacaoCargosNiveisCommand(listaDeNotificacoesParaTratar));            
        }
    }
}
