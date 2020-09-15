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
            IRepositorioNotificacao repositorioNotificacao, IUnitOfWork unitOfWork)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirNotificacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {

            var notificacoes = await repositorioDiarioBordoObservacaoNotificacao.ObterPorDiarioBordoObservacaoId(request.DiarioBordoId);

            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var notificacao in notificacoes)
                {
                    await repositorioDiarioBordoObservacaoNotificacao.Excluir(notificacao);
                    repositorioNotificacao.Remover(notificacao.IdNotificacao);
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
