using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaComUeEDrePorCodigoQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaComUeEDrePorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private ObterTurmaComUeEDrePorCodigoQuery request;

        public ObterTurmaComUeEDrePorCodigoQueryHandler(IRepositorioTurmaConsulta repositorioTurma, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            this.request = request;

            return await Obter();
        }

        protected override string ObterChave()
        {
            return $"turma-ue-dre-codigo:{request.TurmaCodigo}";
        }

        protected override async Task<Turma> ObterObjetoRepositorio()
        {
            return await repositorioTurma.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo);
        }
    }
}
