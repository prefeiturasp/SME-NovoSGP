using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSomenteTurmaPorIdQueryHandler : IRequestHandler<ObterSomenteTurmaPorIdQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterSomenteTurmaPorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Turma> Handle(ObterSomenteTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterSomenteTurmaPorId(request.TurmaId);
        }
    }
}
