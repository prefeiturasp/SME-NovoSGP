using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsQuery, IEnumerable<ComponenteCurricular>>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        public ObterComponentesCurricularesPorIdsQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }
        public async Task<IEnumerable<ComponenteCurricular>> Handle(ObterComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.ObterPorIdsAsync(request.Ids);
        }
    }
}
