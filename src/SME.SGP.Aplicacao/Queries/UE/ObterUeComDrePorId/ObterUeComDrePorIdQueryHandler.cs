using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUeComDrePorIdQueryHandler : IRequestHandler<ObterUeComDrePorIdQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUeComDrePorIdQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUeComDrePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterUeComDrePorId(request.UeId);
        }
    }
}
