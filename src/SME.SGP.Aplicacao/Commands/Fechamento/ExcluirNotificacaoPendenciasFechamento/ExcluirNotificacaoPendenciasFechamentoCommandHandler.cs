using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPendenciasFechamentoCommandHandler : IRequestHandler<ExcluirNotificacaoPendenciasFechamentoCommand, bool>
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ExcluirNotificacaoPendenciasFechamentoCommandHandler(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<bool> Handle(ExcluirNotificacaoPendenciasFechamentoCommand request, CancellationToken cancellationToken)
        {
            var notificacoes = await this.repositorioNotificacao.ObterIdsAsync(request.TurmaCodigo, NotificacaoCategoria.Aviso, NotificacaoTipo.Fechamento, request.Ano);
            if (notificacoes.Any())
            {
                await this.repositorioNotificacao.ExcluirLogicamentePorIdsAsync(notificacoes);
                return true;
            }
            return false;
        }

    }
}
