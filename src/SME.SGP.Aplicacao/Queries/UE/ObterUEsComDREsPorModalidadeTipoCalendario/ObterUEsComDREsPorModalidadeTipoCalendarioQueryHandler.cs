using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsComDREsPorModalidadeTipoCalendarioQueryHandler : IRequestHandler<ObterUEsComDREsPorModalidadeTipoCalendarioQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUEsComDREsPorModalidadeTipoCalendarioQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public Task<IEnumerable<Ue>> Handle(ObterUEsComDREsPorModalidadeTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            return repositorioUe.ObterUEsComDREsPorModalidadeTipoCalendarioQuery(request.Modalidades, request.AnoLetivo);
        }
    }
}
