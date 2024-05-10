using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigoPorIdQueryHandler : IRequestHandler<ObterTurmaCodigoPorIdQuery, string>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmaCodigoPorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<string> Handle(ObterTurmaCodigoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmaCodigoPorId(request.TurmaId);
        }
    }
}
