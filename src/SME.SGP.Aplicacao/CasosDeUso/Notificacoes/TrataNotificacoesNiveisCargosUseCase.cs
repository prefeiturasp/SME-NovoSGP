using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataNotificacoesNiveisCargosUseCase : ITrataNotificacoesNiveisCargosUseCase
    {
        private readonly IMediator mediator;

        public TrataNotificacoesNiveisCargosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit param)
        {
            var listaDeNotificacoesParaTratar = await mediator.Send(new ObterNotificacoesNiveisCargosQuery());
            await mediator.Send(new TrataNotificacaoCargosNiveisCommand(listaDeNotificacoesParaTratar));

            return true;
        }

    }
}

