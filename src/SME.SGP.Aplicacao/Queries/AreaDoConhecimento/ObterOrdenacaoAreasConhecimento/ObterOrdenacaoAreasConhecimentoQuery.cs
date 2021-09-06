using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.ObterOrdenacaoAreasConhecimento
{
    public class ObterOrdenacaoAreasConhecimentoQuery : IRequest<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>
    {
        public ObterOrdenacaoAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurricularesTurma,
                                                    IEnumerable<AreaDoConhecimentoDto> areasConhecimento)
        {
            ComponentesCurricularesTurma = componentesCurricularesTurma;
            AreasConhecimento = areasConhecimento;
        }

        public IEnumerable<DisciplinaDto> ComponentesCurricularesTurma { get; set; }
        public IEnumerable<AreaDoConhecimentoDto> AreasConhecimento { get; set; }

    }
}
