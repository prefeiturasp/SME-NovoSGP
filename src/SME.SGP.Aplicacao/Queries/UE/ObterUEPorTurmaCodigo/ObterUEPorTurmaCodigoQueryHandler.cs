using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaCodigoQueryHandler: IRequestHandler<ObterUEPorTurmaCodigoQuery, Ue>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUEPorTurmaCodigoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public Task<Ue> Handle(ObterUEPorTurmaCodigoQuery request, CancellationToken cancellationToken)
            => Task.FromResult(repositorioUe.ObterUEPorTurma(request.TurmaCodigo));
    }
}
