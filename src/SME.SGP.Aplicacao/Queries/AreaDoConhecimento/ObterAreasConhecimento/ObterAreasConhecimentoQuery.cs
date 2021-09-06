using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterAreasConhecimento
{
    public class ObterAreasConhecimentoQuery : IRequest<IEnumerable<AreaDoConhecimentoDto>>
    {
        public ObterAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurriculares)
        {
            ComponentesCurriculares = componentesCurriculares;
        }

        public IEnumerable<DisciplinaDto> ComponentesCurriculares { get; set; }
    }
}
