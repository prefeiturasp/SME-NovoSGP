using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeNotificacoesNaoLidasPorUsuarioQueryHandler : IRequestHandler<ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery, int>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacao;

        public ObterQuantidadeNotificacoesNaoLidasPorUsuarioQueryHandler(IRepositorioNotificacaoConsulta repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<int> Handle(ObterQuantidadeNotificacoesNaoLidasPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNotificacao.ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoEUsuarioAsync(request.AnoLetivo, request.CodigoRf);
        }
    }
}
