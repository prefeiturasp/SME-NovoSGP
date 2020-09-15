using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDiarioBordoCommandHandler : IRequestHandler<ExcluirNotificacaoDiarioBordoCommand, bool>
    {
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IUnitOfWork unitOfWork;
        public ExcluirNotificacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao,
                                                           IRepositorioNotificacao repositorioNotificacao,
                                                           IUnitOfWork unitOfWork)
        {
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacaoNotificacao));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        // observacao <- notificacao_observacao -> notificacao
        public async Task<bool> Handle(ExcluirNotificacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {

            var notificacoesObservacao = await repositorioDiarioBordoObservacaoNotificacao.ObterPorDiarioBordoObservacaoId(request.ObservacaoId);

            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var notificacaoObservacao in notificacoesObservacao)
                {
                    await repositorioDiarioBordoObservacaoNotificacao.Excluir(notificacaoObservacao);
                    await repositorioNotificacao.RemoverLogico(notificacaoObservacao.IdNotificacao);
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
