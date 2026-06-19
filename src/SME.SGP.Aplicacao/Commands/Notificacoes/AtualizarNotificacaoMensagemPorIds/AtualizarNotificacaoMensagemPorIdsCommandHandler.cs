using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class AtualizarnotificacaoMensagemPorIdsCommandHandler : IRequestHandler<AtualizarNotificacaoMensagemPorIdsCommand>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public AtualizarnotificacaoMensagemPorIdsCommandHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ??
                                          throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task Handle(AtualizarNotificacaoMensagemPorIdsCommand request, CancellationToken cancellationToken)
        {
            await repositorioNotificacao.AtualizarMensagemPorWorkFlowAprovacao(request.Ids, request.Mensagem);
        }
    }
}