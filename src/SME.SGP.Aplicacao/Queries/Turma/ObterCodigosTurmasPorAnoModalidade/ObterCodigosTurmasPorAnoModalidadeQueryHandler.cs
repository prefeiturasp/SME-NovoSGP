using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasPorAnoModalidadeQueryHandler : IRequestHandler<ObterCodigosTurmasPorAnoModalidadeQuery, IEnumerable<string>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterCodigosTurmasPorAnoModalidadeQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<string>> Handle(ObterCodigosTurmasPorAnoModalidadeQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterCodigosTurmasPorAnoModalidade(request.AnoLetivo, request.Modalidades.Select(a => (int)a).ToArray(), request.TurmaCodigo);
    }
}
