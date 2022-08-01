using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaComUeEDrePorIdQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaComUeEDrePorIdQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private ObterTurmaComUeEDrePorIdQuery request;

        public ObterTurmaComUeEDrePorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorIdQuery request, CancellationToken cancellationToken)
        {
            this.request = request;

            return await Obter();
        }

        protected override string ObterChave()
        {
            return $"turma-ue-dre-id:{request.TurmaId}";
        }

        protected override async Task<Turma> ObterObjetoRepositorio()
        {
            return await repositorioTurmaConsulta.ObterTurmaComUeEDrePorId(request.TurmaId);
        }
    }
}
