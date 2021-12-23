using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPlanoAEEObservacaoCommandHandler : IRequestHandler<ExcluirNotificacaoPlanoAEEObservacaoCommand, bool>
    {
        private readonly IRepositorioNotificacaoPlanoAEEObservacao repositorioNotificacaoPlanoAEEObservacao;
        private readonly IMediator mediator;

        public ExcluirNotificacaoPlanoAEEObservacaoCommandHandler(IRepositorioNotificacaoPlanoAEEObservacao repositorioNotificacaoPlanoAEEObservacao, IMediator mediator)
        {
            this.repositorioNotificacaoPlanoAEEObservacao = repositorioNotificacaoPlanoAEEObservacao ?? throw new ArgumentNullException(nameof(repositorioNotificacaoPlanoAEEObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirNotificacaoPlanoAEEObservacaoCommand request, CancellationToken cancellationToken)
        {
            var notificacoesObservacao = await repositorioNotificacaoPlanoAEEObservacao.ObterPorObservacaoPlanoAEEId(request.ObservacaoPlanoId);

            foreach(var notificacaoObservacao in notificacoesObservacao)
            {
                repositorioNotificacaoPlanoAEEObservacao.Remover(notificacaoObservacao.Id);
                await mediator.Send(new ExcluirNotificacaoPorIdCommand(notificacaoObservacao.NotificacaoId));
            }

            return true;
        }
    }
}
