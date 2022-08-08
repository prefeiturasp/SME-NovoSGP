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
    public class ObterTurmaPorCodigoQueryHandler : CacheQuery<Turma>, IRequestHandler<ObterTurmaPorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IMediator mediator;
        private long id;

        public ObterTurmaPorCodigoQueryHandler(
                        IRepositorioTurmaConsulta repositorioTurmaConsulta, 
                        IRepositorioCache repositorioCache,
                        IMediator mediator) : base(repositorioCache)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<Turma> Handle(ObterTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            this.id = await repositorioTurmaConsulta.ObterTurmaIdPorCodigo(request.TurmaCodigo);

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
