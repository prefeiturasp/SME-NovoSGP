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
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IUnitOfWork unitOfWork;

        public ExcluirNotificacaoDevolutivaCommandHandler(IRepositorioNotificacao repositorioNotificacao,
            IRepositorioNotificacaoDevolutiva repositorioNotificacaoDevolutiva,
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioNotificacaoDevolutiva = repositorioNotificacaoDevolutiva ?? throw new ArgumentNullException(nameof(repositorioNotificacaoDevolutiva));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
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
                    repositorioNotificacao.Remover(notificacao.NotificacaoId);                    
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
