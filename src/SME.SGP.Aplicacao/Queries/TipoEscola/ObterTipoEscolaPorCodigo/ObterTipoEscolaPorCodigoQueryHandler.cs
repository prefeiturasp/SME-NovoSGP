using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoEscolaPorCodigoQueryHandler : IRequestHandler<ObterTipoEscolaPorCodigoQuery, TipoEscolaEol>
    {
        private readonly IRepositorioTipoEscola repositorioTipoEscola;

        public ObterTipoEscolaPorCodigoQueryHandler(IRepositorioTipoEscola repositorioTipoEscola)
        {
            this.repositorioTipoEscola = repositorioTipoEscola ?? throw new System.ArgumentNullException(nameof(repositorioTipoEscola));
        }
        public async Task<TipoEscolaEol> Handle(ObterTipoEscolaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTipoEscola.ObterPorCodigoAsync(request.Codigo);
        }
    }
}
