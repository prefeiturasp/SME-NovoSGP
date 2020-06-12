using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Ciclos.ObterCiclosPorUeModalidade
{
    public class ObterCiclosPorUeModalidadeQueryHandler : IRequestHandler<ObterCiclosPorUeModalidadeQuery, IEnumerable<CicloDto>>
    {
        private readonly IRepositorioCiclo repositorioCiclo;

        public ObterCiclosPorUeModalidadeQueryHandler(IRepositorioCiclo repositorioCiclo)
        {
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public Task<IEnumerable<CicloDto>> Handle(ObterCiclosPorUeModalidadeQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
