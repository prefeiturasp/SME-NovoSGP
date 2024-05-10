using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarCacheAulaPrevistaCommandHandler : IRequestHandler<CriarCacheAulaPrevistaCommand, IEnumerable<AulaPrevista>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta;

        public CriarCacheAulaPrevistaCommandHandler(IRepositorioCache repositorioCache, IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.repositorioAulaPrevistaConsulta = repositorioAulaPrevistaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioAulaPrevistaConsulta));
        }

        public async Task<IEnumerable<AulaPrevista>> Handle(CriarCacheAulaPrevistaCommand request, CancellationToken cancellationToken)
        {
            var aulasPrevistas = await repositorioAulaPrevistaConsulta.ObterAulasPrevistasPorUe(request.CodigoUe);
            await repositorioCache.SalvarAsync(request.NomeChave, aulasPrevistas);

            return aulasPrevistas;
        }
    }
}