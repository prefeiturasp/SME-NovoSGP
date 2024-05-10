using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCicloPorAnoModalidadeQueryHandler : IRequestHandler<ObterCicloPorAnoModalidadeQuery,CicloDto>
    {
        private readonly IRepositorioCiclo repositorioCiclo;

        public ObterCicloPorAnoModalidadeQueryHandler(IRepositorioCiclo ciclo)
        {
            repositorioCiclo = ciclo ?? throw new ArgumentNullException(nameof(ciclo));
        }

        public async Task<CicloDto> Handle(ObterCicloPorAnoModalidadeQuery request, CancellationToken cancellationToken)
        {
            return  await repositorioCiclo.ObterCicloPorAnoModalidade(request.Ano, request.Modalidade);
        }
    }
}