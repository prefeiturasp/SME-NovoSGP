using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase : IObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase
    {
        private readonly IMediator mediator;

        public ObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<int> Executar()
        {
            var usuarioLogadoRF = await mediator.Send(new ObterUsuarioLogadoRFQuery());
            return await mediator.Send(new ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery(DateTime.Now.Year, usuarioLogadoRF));
        }
    }
}
