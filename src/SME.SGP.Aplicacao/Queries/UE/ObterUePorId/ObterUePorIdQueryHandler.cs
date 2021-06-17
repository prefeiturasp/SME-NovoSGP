using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUePorIdQueryHandler : IRequestHandler<ObterUePorIdQuery, Ue>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUePorIdQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUePorIdQuery request, CancellationToken cancellationToken)
                => await repositorioUe.ObterUeComDrePorId(request.Id);
    }
}
