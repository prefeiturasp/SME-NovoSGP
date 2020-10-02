using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorIdQueryHandler : IRequestHandler<ObterTurmaPorIdQuery, Turma>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaPorIdQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }
        public async Task<Turma> Handle(ObterTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterPorId(request.TurmaId);
        }
    }
}
