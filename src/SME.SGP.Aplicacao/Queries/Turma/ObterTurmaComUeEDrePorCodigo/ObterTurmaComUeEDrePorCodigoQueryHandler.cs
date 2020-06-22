using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaComUeEDrePorCodigoQueryHandler : IRequestHandler<ObterTurmaComUeEDrePorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurma repositorioTurma;
        public ObterTurmaComUeEDrePorCodigoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo);
    }
}
