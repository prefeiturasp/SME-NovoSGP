using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDevolutivaCommandHandler : IRequestHandler<ExcluirNotificacaoDevolutivaCommand, bool>
    {
        private readonly IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public ExcluirNotificacaoDevolutivaCommandHandler(IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva,
                                                          IMediator mediator,
                                                          IUnitOfWork unitOfWork)
        {
            this.repositorioNotificacaoDevolutiva = repositorioNotificacaoDevolutiva ?? throw new ArgumentNullException(nameof(repositorioNotificacaoDevolutiva));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirNotificacaoDevolutivaCommand request, CancellationToken cancellationToken)
        {
            var notificacoes = await repositorioNotificacaoDevolutiva.ObterPorDevolutivaId(request.DevolutivaId);

            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var notificacao in notificacoes)
                {
                    await repositorioNotificacaoDevolutiva.Excluir(notificacao);
                    await mediator.Send(new ExcluirNotificacaoPorIdCommand(notificacao.NotificacaoId));
                }

                unitOfWork.PersistirTransacao();
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }      

            return true;
        }
    }
}
