using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasCalendarioPorUeQueryHandler : IRequestHandler<ObterPendenciasCalendarioPorUeQuery, IEnumerable<long>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterPendenciasCalendarioPorUeQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public Task<IEnumerable<long>> Handle(ObterPendenciasCalendarioPorUeQuery request, CancellationToken cancellationToken)
        {
            return repositorioUe.ObterPendenciasCalendarioPorAnoLetivoUe(request.AnoLetivo, request.UeId);
        }
    }
}