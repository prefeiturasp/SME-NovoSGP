using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPrevistasPorCodigoUeQueryHandler : IRequestHandler<ObterAulasPrevistasPorCodigoUeQuery, IEnumerable<AulaPrevista>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta;
        public ObterAulasPrevistasPorCodigoUeQueryHandler(IRepositorioCache repositorioCache, IRepositorioAulaPrevistaConsulta repositorioAulaPrevistaConsulta)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.repositorioAulaPrevistaConsulta = repositorioAulaPrevistaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioAulaPrevistaConsulta));
        }

        public Task<IEnumerable<AulaPrevista>> Handle(ObterAulasPrevistasPorCodigoUeQuery request, CancellationToken cancellationToken)
        {
            if (request.ObterPorCache)
            {
                var nomeChave = string.Format(NomeChaveCache.AULAS_PREVISTAS_UE, request.CodigoUe);

                return repositorioCache.ObterAsync(nomeChave,
                     async () => await repositorioAulaPrevistaConsulta.ObterAulasPrevistasPorUe(request.CodigoUe),
                     "Obter aula previstas por código");
            }

            return repositorioAulaPrevistaConsulta.ObterAulasPrevistasPorUe(request.CodigoUe);
        }
    }
}
