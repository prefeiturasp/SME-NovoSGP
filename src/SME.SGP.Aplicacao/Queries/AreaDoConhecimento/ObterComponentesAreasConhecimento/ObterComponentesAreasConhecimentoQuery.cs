using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterComponentesAreasConhecimento
{
    public class ObterComponentesAreasConhecimentoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurricularesTurma,
                                                      IEnumerable<AreaDoConhecimentoDto> areasConhecimento)
        {
            ComponentesCurricularesTurma = componentesCurricularesTurma ?? Enumerable.Empty<DisciplinaDto>();
            AreasConhecimento = areasConhecimento ?? Enumerable.Empty<AreaDoConhecimentoDto>();
        }

        public IEnumerable<DisciplinaDto> ComponentesCurricularesTurma { get; private set; }
        public IEnumerable<AreaDoConhecimentoDto> AreasConhecimento { get; private set; }
    }
}
