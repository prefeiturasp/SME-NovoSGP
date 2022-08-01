using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaIdQueryHandler : CacheQuery<Ue>, IRequestHandler<ObterUEPorTurmaIdQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;
        private ObterUEPorTurmaIdQuery request;

        public ObterUEPorTurmaIdQueryHandler(IRepositorioUeConsulta repositorioUe, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUEPorTurmaIdQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterUEPorTurmaId(request.TurmaId);

        protected override string ObterChave()
        {
            return $"ue-turma-id:{request.TurmaId}";
        }

        protected override async Task<Ue> ObterObjetoRepositorio()
        {
            return await repositorioUe.ObterUEPorTurmaId(request.TurmaId);
        }
    }
}
