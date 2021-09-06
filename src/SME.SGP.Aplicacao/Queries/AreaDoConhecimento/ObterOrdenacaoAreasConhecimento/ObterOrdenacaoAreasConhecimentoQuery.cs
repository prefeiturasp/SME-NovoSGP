using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterOrdenacaoAreasConhecimento
{
    public class ObterOrdenacaoAreasConhecimentoQuery : IRequest<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>
    {
        public ObterOrdenacaoAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurricularesTurma,
                                                    IEnumerable<AreaDoConhecimentoDto> areasConhecimento)
        {
            ComponentesCurricularesTurma = componentesCurricularesTurma ?? Enumerable.Empty<DisciplinaDto>();
            AreasConhecimento = areasConhecimento ?? Enumerable.Empty<AreaDoConhecimentoDto>();
        }

        public IEnumerable<DisciplinaDto> ComponentesCurricularesTurma { get; private set; }
        public IEnumerable<AreaDoConhecimentoDto> AreasConhecimento { get; private set; }
    }
}
