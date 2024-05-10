using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Sentry;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaComUeEDrePorIdQueryHandler : IRequestHandler<ObterTurmaComUeEDrePorIdQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IRepositorioCache repositorioCache;

        public ObterTurmaComUeEDrePorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache) 
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(ObterChave(request.TurmaId), async () => await repositorioTurmaConsulta.ObterTurmaComUeEDrePorId(request.TurmaId));
        }

        private string ObterChave(long turmaId)
        {
            return string.Format(NomeChaveCache.TURMA_UE_DRE_ID, turmaId); 
        }
    }
}
