using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterAreasConhecimento
{
    public class ObterAreasConhecimentoQuery : IRequest<IEnumerable<AreaDoConhecimentoDto>>
    {
        public ObterAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurriculares)
        {
            ComponentesCurriculares = componentesCurriculares ?? Enumerable.Empty<DisciplinaDto>();
        }

        public IEnumerable<DisciplinaDto> ComponentesCurriculares { get; private set; }
    }
}
