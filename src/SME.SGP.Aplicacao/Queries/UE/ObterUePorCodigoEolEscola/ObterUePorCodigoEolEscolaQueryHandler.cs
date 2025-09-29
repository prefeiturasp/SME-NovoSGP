using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.UE.ObterUePorCodigoEolEscola
{
    public class ObterUePorCodigoEolEscolaQueryHandler : IRequestHandler<ObterUePorCodigoEolEscolaQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUePorCodigoEolEscolaQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUePorCodigoEolEscolaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterPorCodigoAsync(request.CodigoEolEscola);
        }
    }
}
