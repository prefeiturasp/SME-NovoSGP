using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasIdPorUeCodigoEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasIdPorUeCodigoEAnoLetivoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmasIdPorUeCodigoEAnoLetivoQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<long>> Handle(ObterTurmasIdPorUeCodigoEAnoLetivoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasIdsPorUeEAnoLetivo(request.AnoLetivo, request.UeCodigo);
    }
}
