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
    public class ObterTurmaComUeEDrePorIdQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaComUeEDrePorIdQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private long id;

        public ObterTurmaComUeEDrePorIdQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorIdQuery request, CancellationToken cancellationToken)
        {
            this.id = request.TurmaId;

            return await Obter();
        }

        protected override string ObterChave()
        {
            return string.Format(NomeChaveCache.CHAVE_TURMA_ID, id); 
        }

        protected override async Task<Turma> ObterObjetoRepositorio()
        {
            return await repositorioTurmaConsulta.ObterTurmaComUeEDrePorId(id);
        }
    }
}
