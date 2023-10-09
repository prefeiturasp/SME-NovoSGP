using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInfoPedagogicasComponentesCurricularesPorIdsQueryHandler : IRequestHandler<ObterInfoPedagogicasComponentesCurricularesPorIdsQuery, IEnumerable<InfoComponenteCurricular>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;


        public ObterInfoPedagogicasComponentesCurricularesPorIdsQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                                                             IRepositorioCache repositorioCache,
                                                                             IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<InfoComponenteCurricular>> Handle(ObterInfoPedagogicasComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
        {
            var todosComponentesCurriculares = (await repositorioCache.ObterAsync("InformacoesComponentesCurriculares",
                                                                                   async () => await repositorioComponenteCurricular.ObterInformacoesComponentesCurriculares()))?.ToList();
            
            if (todosComponentesCurriculares.EhNulo())
                return Enumerable.Empty<InfoComponenteCurricular>();

            if (request.Ids.NaoEhNulo() && request.Ids.Any())
                return todosComponentesCurriculares.Where(cc => request.Ids.Contains(cc.Codigo));

            return todosComponentesCurriculares;
        }
    }
}
