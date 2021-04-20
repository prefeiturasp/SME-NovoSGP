using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCicloPorCodigoQueryHandler : IRequestHandler<ObterCicloPorCodigoQuery, CicloEnsino>
    {
        private readonly IRepositorioCiclo repositorioCiclo;

        public ObterCicloPorCodigoQueryHandler(IRepositorioCiclo repositorioCiclo)
        {
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public async Task<CicloEnsino> Handle(ObterCicloPorCodigoQuery request, CancellationToken cancellationToken)
        {
            var ciclo = await repositorioCiclo.ObterCicloPorCodigoEol(request.CodigoEol);

            return ciclo;
        }
    }
}
