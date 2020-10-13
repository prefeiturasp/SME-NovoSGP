using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimasNotificacoesNaoLidasPorUsuarioUseCase : IObterUltimasNotificacoesNaoLidasPorUsuarioUseCase
    {
        private readonly IMediator mediator;

        public ObterUltimasNotificacoesNaoLidasPorUsuarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotificacaoBasicaDto>> Executar(bool tituloReduzido)
        {
            //TODO: Utilizar query que retorna apenas o RF quando for feito merge com dev release
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            return await mediator.Send(new ObterUltimasNotificacoesNaoLidasPorUsuarioQuery(DateTime.Now.Year, usuarioLogado.CodigoRf, tituloReduzido));
        }
    }
}
