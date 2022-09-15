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
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public ExcluirNotificacaoCartaIntencoesObservacaoCommandHandler(IRepositorioNotificacaoCartaIntencoesObservacao repositorioNotificacaoCartaIntencoesObservacao,
                                                                        IMediator mediator,
                                                                        IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNotificacaoCartaIntencoesObservacao = repositorioNotificacaoCartaIntencoesObservacao ?? throw new ArgumentNullException(nameof(repositorioNotificacaoCartaIntencoesObservacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirNotificacaoCartaIntencoesObservacaoCommand request, CancellationToken cancellationToken)
        {
            var notificacoes = await repositorioNotificacaoCartaIntencoesObservacao.ObterPorCartaIntencoesObservacaoId(request.CartaIntencoesObservacaoId);

            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var notificacao in notificacoes)
                {
                    await repositorioNotificacaoCartaIntencoesObservacao.Excluir(notificacao);
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
