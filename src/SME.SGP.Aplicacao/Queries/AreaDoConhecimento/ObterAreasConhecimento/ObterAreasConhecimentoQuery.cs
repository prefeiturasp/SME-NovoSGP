using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterAreasConhecimentoQuery : IRequest<IEnumerable<AreaDoConhecimentoDto>>
    {
        public ObterAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurriculares, bool consideraRegencia = true)
        {
            ComponentesCurriculares = componentesCurriculares ?? Enumerable.Empty<DisciplinaDto>();
            ConsideraRegencia = consideraRegencia;
        }

        public IEnumerable<DisciplinaDto> ComponentesCurriculares { get; private set; }
        public bool ConsideraRegencia { get; set; }
    }
}
