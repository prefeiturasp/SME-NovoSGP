using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarNotificacaoPlanoAEECommandHandler : IRequestHandler<GerarNotificacaoPlanoAEECommand, bool>
    {
        private readonly IRepositorioNotificacaoPlanoAEE repositorioNotificacaoPlanoAEE;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public GerarNotificacaoPlanoAEECommandHandler(IRepositorioNotificacaoPlanoAEE repositorioNotificacaoPlanoAEE, IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioNotificacaoPlanoAEE = repositorioNotificacaoPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioNotificacaoPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(GerarNotificacaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var notificacoes = await mediator.Send(new EnviarNotificacaoUsuariosCommand(request.Titulo, request.Descricao, request.TipoNotificacao, Dominio.NotificacaoTipo.AEE, request.UsuariosIds));
                    foreach(var notificacaoId in notificacoes)
                        await repositorioNotificacaoPlanoAEE.SalvarAsync(new Dominio.NotificacaoPlanoAEE(notificacaoId, request.PlanoId, request.Tipo));

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();

                    throw;
                }            
            }

            return true;
        }
    }
}
