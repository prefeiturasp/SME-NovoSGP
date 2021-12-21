using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUesCodigosPorModalidadeEAnoLetivoQueryHandler : IRequestHandler<ObterUesCodigosPorModalidadeEAnoLetivoQuery, IEnumerable<string>>
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ObterUesCodigosPorModalidadeEAnoLetivoQueryHandler(IRepositorioUeConsulta repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<string>> Handle(ObterUesCodigosPorModalidadeEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterUesCodigosPorModalidadeEAnoLetivo(request.Modalidade, request.AnoLetivo);
        }
    }
}
