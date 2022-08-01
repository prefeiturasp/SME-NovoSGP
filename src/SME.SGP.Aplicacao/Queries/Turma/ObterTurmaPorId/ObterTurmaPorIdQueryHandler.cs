using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorIdQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaPorIdQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private ObterTurmaPorIdQuery request;

        public ObterTurmaPorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<Turma> Handle(ObterTurmaPorIdQuery request, CancellationToken cancellationToken)
        {
            this.request = request;

            return await Obter();
        }

        protected override string ObterChave()
        {
            return $"turma-id:{request.TurmaId}";
        }

        protected override async Task<Turma> ObterObjetoRepositorio()
        {
            return await repositorioTurmaConsulta.ObterPorId(request.TurmaId);
        }
    }
}
