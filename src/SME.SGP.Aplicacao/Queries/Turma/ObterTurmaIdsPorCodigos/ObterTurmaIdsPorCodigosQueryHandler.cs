using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaIdsPorCodigosQueryHandler : IRequestHandler<ObterTurmaIdsPorCodigosQuery, IEnumerable<long>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmaIdsPorCodigosQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<long>> Handle(ObterTurmaIdsPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterIdsPorCodigos(request.TurmasCodigo);
        }
    }
}
