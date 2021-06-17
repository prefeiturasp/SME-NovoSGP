
using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeUesPorAnoLetivoQueryHandler : IRequestHandler<ObterQuantidadeUesPorAnoLetivoQuery, int>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterQuantidadeUesPorAnoLetivoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
        }
        public async Task<int> Handle(ObterQuantidadeUesPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterQuantidadeUesPorAnoLetivoAsync(request.AnoLetivo);
        }
    }
}
