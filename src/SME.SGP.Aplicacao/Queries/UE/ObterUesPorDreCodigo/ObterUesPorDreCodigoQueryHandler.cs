using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesPorDreCodigoQueryHeandler : IRequestHandler<ObterUesPorDreCodigoQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUesPorDreCodigoQueryHeandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUesPorDreCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterPorDre(request.DreId);
        }
    }
}
