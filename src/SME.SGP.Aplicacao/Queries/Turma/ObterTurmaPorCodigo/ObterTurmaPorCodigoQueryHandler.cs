using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorCodigoQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaPorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private ObterTurmaPorCodigoQuery request;

        public ObterTurmaPorCodigoQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        
        public async Task<Turma> Handle(ObterTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            this.request = request;

            return await Obter();
        }

        protected override string ObterChave()
        {
            return $"turma-codigo:{request.TurmaCodigo}";
        }

        protected override async Task<Turma> ObterObjetoRepositorio()
        {
            return await repositorioTurmaConsulta.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo);
        }
    }
}
