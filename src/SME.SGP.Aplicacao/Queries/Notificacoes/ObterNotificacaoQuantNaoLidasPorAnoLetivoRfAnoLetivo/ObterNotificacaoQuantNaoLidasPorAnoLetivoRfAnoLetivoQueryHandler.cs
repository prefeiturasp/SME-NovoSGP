using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQueryHandler : IRequestHandler<ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery, int>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacao;

        public ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQueryHandler(IRepositorioNotificacaoConsulta repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<int> Handle(ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNotificacao.ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoEUsuarioAsync(request.Ano, request.UsuarioRf);
        }
    }
}