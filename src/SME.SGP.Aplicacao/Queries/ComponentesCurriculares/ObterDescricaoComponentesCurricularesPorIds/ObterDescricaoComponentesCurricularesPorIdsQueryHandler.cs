using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDescricaoComponentesCurricularesPorIdsQueryHandler : IRequestHandler<ObterDescricaoComponentesCurricularesPorIdsQuery, IEnumerable<ComponenteCurricularSimplesDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public ObterDescricaoComponentesCurricularesPorIdsQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<IEnumerable<ComponenteCurricularSimplesDto>> Handle(ObterDescricaoComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
            => await repositorioComponenteCurricular.ObterDescricaoPorIds(request.Ids);
    }
}
