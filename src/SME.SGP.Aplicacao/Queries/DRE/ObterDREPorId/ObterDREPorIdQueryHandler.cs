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
    public class ObterDREPorIdQueryHandler : IRequestHandler<ObterDREPorIdQuery, Dre>
    {
        private readonly IRepositorioDreConsulta repositorioDre;
        private readonly IRepositorioCache repositorioCache;

        public ObterDREPorIdQueryHandler(IRepositorioDreConsulta repositorioDre, IRepositorioCache repositorioCache)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Dre> Handle(ObterDREPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(ObterChave(request.DreId),
                async () => await repositorioDre.ObterPorIdAsync(request.DreId),
                "Obter DRE");
        }
        private string ObterChave(long id)
        {
            return string.Format(NomeChaveCache.DRE_ID, id);
        }
    }
}