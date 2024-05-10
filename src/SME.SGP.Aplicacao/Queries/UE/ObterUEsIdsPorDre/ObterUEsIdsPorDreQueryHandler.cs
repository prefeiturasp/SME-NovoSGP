using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsIdsPorDreQueryHandler : IRequestHandler<ObterUEsIdsPorDreQuery, IEnumerable<long>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUEsIdsPorDreQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public Task<IEnumerable<long>> Handle(ObterUEsIdsPorDreQuery request, CancellationToken cancellationToken)
            => repositorioUe.ObterIdsPorDre(request.DreId);
    }
}
