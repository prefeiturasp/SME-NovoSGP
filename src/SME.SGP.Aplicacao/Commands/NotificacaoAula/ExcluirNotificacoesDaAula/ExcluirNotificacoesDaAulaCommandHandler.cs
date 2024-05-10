using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacoesDaAulaCommandHandler : IRequestHandler<ExcluirNotificacoesDaAulaCommand, bool>
    {
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;
        private readonly IMediator mediator;

        public ExcluirNotificacoesDaAulaCommandHandler(IRepositorioNotificacaoAula repositorioNotificacaoAula,
                                                       IMediator mediator)
        {
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirNotificacoesDaAulaCommand request, CancellationToken cancellationToken)
        {
            foreach (var notificacaoAula in await repositorioNotificacaoAula.ObterPorAulaAsync(request.AulaId))
            {
                await repositorioNotificacaoAula.Excluir(notificacaoAula);
                await mediator.Send(new ExcluirNotificacaoPorIdCommand(notificacaoAula.NotificacaoId));
            }

            return true;
        }
    }
}
