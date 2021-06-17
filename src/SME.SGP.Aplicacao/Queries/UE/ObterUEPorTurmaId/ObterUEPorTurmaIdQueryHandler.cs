using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaIdQueryHandler : IRequestHandler<ObterUEPorTurmaIdQuery, Ue>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUEPorTurmaIdQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUEPorTurmaIdQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterUEPorTurmaId(request.TurmaId);
    }
}
