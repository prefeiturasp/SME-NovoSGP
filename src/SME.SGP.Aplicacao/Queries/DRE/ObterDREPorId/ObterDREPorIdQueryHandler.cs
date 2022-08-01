using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDREPorIdQueryHandler : CacheQuery<Dre>, IRequestHandler<ObterDREPorIdQuery, Dre>
    {
        private readonly IRepositorioDreConsulta repositorioDre;
        private ObterDREPorIdQuery request;

        public ObterDREPorIdQueryHandler(IRepositorioDreConsulta repositorioDre, IRepositorioCache repositorioCache) : base(repositorioCache)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<Dre> Handle(ObterDREPorIdQuery request, CancellationToken cancellationToken)
        {
            this.request = request;

            return await Obter();
        }

        protected override string ObterChave()
        {
            return $"Dre-Id:{request.DreId}";
        }

        protected override async Task<Dre> ObterObjetoRepositorio()
        {
            return await repositorioDre.ObterPorIdAsync(request.DreId);
        }
    }
}