using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasPorUeAnoQueryHandler : IRequestHandler<ObterCodigosTurmasPorUeAnoQuery, IEnumerable<string>>
    {
        private readonly IRepositorioTurmaConsulta repositorio;

        public ObterCodigosTurmasPorUeAnoQueryHandler(IRepositorioTurmaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<string>> Handle(ObterCodigosTurmasPorUeAnoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterCodigosTurmasPorUeAno(request.AnoLetivo, request.UeCodigo);
        }
    }
}
