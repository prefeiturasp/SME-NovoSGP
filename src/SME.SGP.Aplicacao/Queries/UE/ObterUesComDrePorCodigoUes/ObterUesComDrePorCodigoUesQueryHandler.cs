using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterUesComDrePorCodigoUesQueryHandler : IRequestHandler<ObterUesComDrePorCodigoUesQuery,IEnumerable<Ue>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUesComDrePorCodigoUesQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUesComDrePorCodigoUesQuery request, CancellationToken cancellationToken)
            =>  await repositorioUe.ObterUEsComDREsPorCodigoUes(request.UesCodigos);
        
    }
}