using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeComDrePorCodigoQueryHandler : IRequestHandler<ObterUeComDrePorCodigoQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUeComDrePorCodigoQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUeComDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterUeComDrePorCodigo(request.UeCodigo);
        }
    }
}
