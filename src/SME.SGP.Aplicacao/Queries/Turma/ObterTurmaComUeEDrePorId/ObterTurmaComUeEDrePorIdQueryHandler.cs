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
    public class ObterTurmaComUeEDrePorIdQueryHandler : IRequestHandler<ObterTurmaComUeEDrePorIdQuery, Turma>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaComUeEDrePorIdQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorIdQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmaComUeEDrePorId(request.TurmaId);
    }
}
