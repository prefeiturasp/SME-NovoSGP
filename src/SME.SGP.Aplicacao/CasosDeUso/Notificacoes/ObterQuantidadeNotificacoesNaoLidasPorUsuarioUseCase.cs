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
            //TODO: Utilizar query que retorna apenas o RF quando for feito merge com dev release
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            return await mediator.Send(new ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery(DateTime.Now.Year, usuarioLogado.CodigoRf));
        }
    }
}
