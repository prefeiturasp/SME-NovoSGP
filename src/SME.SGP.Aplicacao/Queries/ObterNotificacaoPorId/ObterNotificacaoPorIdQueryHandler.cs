using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoPorIdQueryHandler : IRequestHandler<ObterNotificacaoPorIdQuery, Notificacao>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ObterNotificacaoPorIdQueryHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ??
                                          throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<Notificacao> Handle(ObterNotificacaoPorIdQuery request, CancellationToken cancellationToken)
        {
            var notificacao = await repositorioNotificacao.ObterPorIdAsync(request.NotificacaoId);

            if (notificacao.EhNulo())
                throw new NegocioException($"Notificação de Id: '{request.NotificacaoId}' não localizada.");

            return notificacao;
        }
    }
}