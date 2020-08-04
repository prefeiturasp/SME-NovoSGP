using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
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
            var ciclos = await repositorioCiclo.ObterCiclosPorAnoModalidadeECodigoUe(new FiltroCicloPorModalidadeECodigoUeDto(request.Modalidade, request.CodigoUe));
            
            if (ciclos == null || !ciclos.Any())
                throw new NegocioException("Não foi possível obter os ciclos");
            
            return ciclos;
        }
    }
}
