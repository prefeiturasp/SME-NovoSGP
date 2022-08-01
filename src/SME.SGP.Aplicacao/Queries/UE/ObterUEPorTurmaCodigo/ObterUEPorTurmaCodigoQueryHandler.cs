using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEPorTurmaCodigoQueryHandler: CacheQuery<Ue>, IRequestHandler<ObterUEPorTurmaCodigoQuery, Ue>
    {
        private readonly IRepositorioUeConsulta repositorioUe;
        private ObterUEPorTurmaCodigoQuery request;

        public ObterUEPorTurmaCodigoQueryHandler(IRepositorioUeConsulta repositorioUe, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUEPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            this.request = request;

            return await Obter();
        }

        protected override string ObterChave()
        {
            return $"ue-turma-codigo:{request.TurmaCodigo}";
        }

        protected override Task<Ue> ObterObjetoRepositorio()
        {
            return Task.FromResult(repositorioUe.ObterUEPorTurma(request.TurmaCodigo));
        }
    }
}
