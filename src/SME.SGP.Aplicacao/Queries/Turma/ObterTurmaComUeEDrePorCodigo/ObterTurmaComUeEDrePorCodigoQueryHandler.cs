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
    public class ObterTurmaComUeEDrePorCodigoQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaComUeEDrePorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IMediator mediator;
        private long id;

        public ObterTurmaComUeEDrePorCodigoQueryHandler(
                        IRepositorioTurmaConsulta repositorioTurma, 
                        IRepositorioCache repositorioCache,
                        IMediator mediator) : base(repositorioCache)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Turma> Handle(ObterTurmaComUeEDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            this.id = await repositorioTurma.ObterTurmaIdPorCodigo(request.TurmaCodigo);

            return await Obter();
        }

        protected override string ObterChave()
        {
            return string.Format(NomeChaveCache.CHAVE_TURMA_ID, id);
        }

        protected override async Task<Turma> ObterObjetoRepositorio()
        {
            return await this.mediator.Send(new ObterTurmaComUeEDrePorIdQuery(id));
        }
    }
}
