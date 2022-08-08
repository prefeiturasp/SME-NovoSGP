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
    public class ObterTurmaPorIdQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaPorIdQuery, Turma>
    {
        private readonly IMediator mediator;
        private long id;

        public ObterTurmaPorIdQueryHandler(
                    IRepositorioCache repositorioCache,
                    IMediator mediator) : base(repositorioCache)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<Turma> Handle(ObterTurmaPorIdQuery request, CancellationToken cancellationToken)
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
            return await this.mediator.Send(new ObterTurmaComUeEDrePorIdQuery(id));
        }
    }
}
