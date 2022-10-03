using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesSimplesPorIdsQueryHandler : IRequestHandler<ObterComponentesCurricularesSimplesPorIdsQuery, IEnumerable<ComponenteCurricularSimplesDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public ObterComponentesCurricularesSimplesPorIdsQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<IEnumerable<ComponenteCurricularSimplesDto>> Handle(ObterComponentesCurricularesSimplesPorIdsQuery request, CancellationToken cancellationToken)
            => await repositorioComponenteCurricular.ObterComponentesSimplesPorIds(request.Ids);

        
    }
}
