using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDiarioBordoCommandHandler : IRequestHandler<ExcluirNotificacaoDiarioBordoCommand, bool>
    {
        private readonly IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ExcluirNotificacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacaoNotificacao repositorioDiarioBordoObservacaoNotificacao,
                                                           IRepositorioNotificacao repositorioNotificacao,
                                                           IUnitOfWork unitOfWork,
                                                           IMediator mediator)
        {
            this.repositorioDiarioBordoObservacaoNotificacao = repositorioDiarioBordoObservacaoNotificacao ?? throw new ArgumentNullException(nameof(repositorioDiarioBordoObservacaoNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
                    var notificacaoObservacaoDominio = MapearParaDominio(notificacaoObservacao);
                    await repositorioDiarioBordoObservacaoNotificacao.Excluir(notificacaoObservacaoDominio);
                    await mediator.Send(new ExcluirNotificacaoPorIdCommand(notificacaoObservacao.IdNotificacao));
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

        private DiarioBordoObservacaoNotificacao MapearParaDominio(DiarioBordoObservacaoNotificacaoUsuarioDto dto)
        {
            return new DiarioBordoObservacaoNotificacao()
            {
                Id = dto.Id,
                IdNotificacao = dto.IdNotificacao,
                IdObservacao = dto.IdObservacao
            };
        }
    }
}
