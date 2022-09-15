using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoCompensacaoAusenciaCommandHandler : AsyncRequestHandler<ExcluirNotificacaoCompensacaoAusenciaCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNotificacaoCompensacaoAusencia repositorioNotificacaoCompensacaoAusencia;

        public ExcluirNotificacaoCompensacaoAusenciaCommandHandler(IMediator mediator,
                                                                   IRepositorioNotificacaoCompensacaoAusencia repositorioNotificacaoCompensacaoAusencia)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacaoCompensacaoAusencia = repositorioNotificacaoCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioNotificacaoCompensacaoAusencia));
        }

        protected override async Task Handle(ExcluirNotificacaoCompensacaoAusenciaCommand request, CancellationToken cancellationToken)
        {
            var notificacoesCompensacao = await repositorioNotificacaoCompensacaoAusencia.ObterPorCompensacao(request.CompensacaoAusenciaId);
            foreach (var notificacaoCompensacao in notificacoesCompensacao)
            {
                await repositorioNotificacaoCompensacaoAusencia.Excluir(notificacaoCompensacao);

                await mediator.Send(new ExcluirNotificacaoPorIdCommand(notificacaoCompensacao.NotificacaoId));
            }
        }
    }
}
