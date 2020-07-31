using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCiclosPorModalidadeECodigoUeQueryHandler : IRequestHandler<ObterCiclosPorModalidadeECodigoUeQuery, IEnumerable<RetornoCicloDto>>
    {
        private readonly IRepositorioCiclo repositorioCiclo;

        public ObterCiclosPorModalidadeECodigoUeQueryHandler(IRepositorioCiclo repositorioCiclo)
        {
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public async Task<IEnumerable<RetornoCicloDto>> Handle(ObterCiclosPorModalidadeECodigoUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCiclo.ObterCiclosPorAnoModalidadeECodigoUe(new FiltroCicloPorModalidadeECodigoUeDto(request.Modalidade, request.CodigoUe));
        }
    }
}
