using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IServicoEol servicoEol;

        public ObterComponentesCurricularesPorIdsQueryHandler(IRepositorioComponenteCurricular repositorioComponenteCurricular, IServicoEol servicoEol)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new System.ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsQuery request, CancellationToken cancellationToken)
        {
            if (request.PossuiTerritorio.HasValue && request.PossuiTerritorio.Value)
                return await servicoEol.ObterDisciplinasPorIdsAgrupadas(request.Ids);
            else
                return await repositorioComponenteCurricular.ObterDisciplinasPorIds(request.Ids);
        }
    }
}
