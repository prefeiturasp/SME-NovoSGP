using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoCartaIntencoesObservacaoCommandHandler : IRequestHandler<ExcluirNotificacaoCartaIntencoesObservacaoCommand, bool>
    {
        private readonly IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IMediator mediator;

        public ExcluirNotificacaoCartaIntencoesObservacaoCommandHandler(IRepositorioNotificacao repositorioNotificacao,
            IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao,
            IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacaoCartaIntencoesObservacao = repositorioNotificacaoCartaIntencoesObservacao ?? throw new ArgumentNullException(nameof(repositorioNotificacaoCartaIntencoesObservacao));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<bool> Handle(ExcluirNotificacaoCartaIntencoesObservacaoCommand request, CancellationToken cancellationToken)
        {

            var notificacoes = await repositorioNotificacaoCartaIntencoesObservacao.ObterPorCartaIntencoesObservacaoId(request.CartaIntencoesObservacaoId);

            foreach (var notificacao in notificacoes)
            {
                repositorioNotificacao.Remover(notificacao.NotificacaoId);
                await repositorioNotificacaoCartaIntencoesObservacao.Excluir(notificacao);
            }

            return true;
        }
    }
}
