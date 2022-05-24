using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasPorAnoModalidadeUeQueryHandler : IRequestHandler<ObterCodigosTurmasPorAnoModalidadeUeQuery, IEnumerable<string>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        public ObterCodigosTurmasPorAnoModalidadeUeQueryHandler()
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<IEnumerable<string>> Handle(ObterCodigosTurmasPorAnoModalidadeUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterCodigosTurmasPorAnoModalidadeUe(request.AnoLetivo,(int)request.Modalidade,request.UeCodigo);
        }
    }
}
