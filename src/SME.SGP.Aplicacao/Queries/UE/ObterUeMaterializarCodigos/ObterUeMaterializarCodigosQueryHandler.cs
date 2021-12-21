using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeMaterializarCodigosQueryHandler : IRequestHandler<ObterUeMaterializarCodigosQuery, Tuple<List<Ue>, string[]>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUeMaterializarCodigosQueryHandler(IRepositorioUeConsulta repositorio)
        {
            this.repositorioUe = repositorio;
        }

        public async Task<Tuple<List<Ue>, string[]>> Handle(ObterUeMaterializarCodigosQuery request, CancellationToken cancellationToken)
        {
            return repositorioUe.MaterializarCodigosUe(request.IdUes);
        }

    }
}