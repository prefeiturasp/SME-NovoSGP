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
    public class ObterTurmaPorIdQueryHandler : IRequestHandler<ObterTurmaPorIdQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IRepositorioCache repositorioCache;
        
        public ObterTurmaPorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache) 
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<Turma> Handle(ObterTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(ObterChave(request.TurmaId), async () => await repositorioTurmaConsulta.ObterPorId(request.TurmaId));
        }

        protected string ObterChave(long turmaId)
        {
            return string.Format(NomeChaveCache.TURMA_ID, turmaId);
        }
    }
}