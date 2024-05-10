using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    public class ObterNotificacaoPorCodigoQueryHandler : IRequestHandler<ObterNotificacaoPorCodigoQuery, Notificacao>
    {
        private readonly IRepositorioNotificacaoConsulta repositorioNotificacao;

        public ObterNotificacaoPorCodigoQueryHandler(IRepositorioNotificacaoConsulta repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }

        public async Task<Notificacao> Handle(ObterNotificacaoPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioNotificacao.ObterPorCodigo(request.Codigo);
        }
    }
}