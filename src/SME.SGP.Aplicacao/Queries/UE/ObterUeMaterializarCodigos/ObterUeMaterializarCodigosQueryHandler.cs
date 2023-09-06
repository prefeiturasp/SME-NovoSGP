using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeMaterializarCodigosQueryHandler : IRequestHandler<ObterUeMaterializarCodigosQuery, (List<Ue> Ues, string[] CodigosUesNaoEncontradas)>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUeMaterializarCodigosQueryHandler(IRepositorioUeConsulta repositorio)
        {
            this.repositorioUe = repositorio;
        }

        public async Task<(List<Ue> Ues, string[] CodigosUesNaoEncontradas)> Handle(ObterUeMaterializarCodigosQuery request, CancellationToken cancellationToken)
        {
            return repositorioUe.MaterializarCodigosUe(request.IdUes);
        }

    }
}