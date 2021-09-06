using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Queries.AreaDoConhecimento.MapearAreasConhecimento
{
    public class MapearAreasConhecimentoQuery : IRequest<IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>>
    {
        public MapearAreasConhecimentoQuery(IEnumerable<DisciplinaDto> componentesCurricularesTurma,
                                            IEnumerable<AreaDoConhecimentoDto> areasConhecimentos,
                                            IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto> gruposAreaOrdenacao,
                                            long grupoMatrizId)
        {
            ComponentesCurricularesTurma = componentesCurricularesTurma ?? Enumerable.Empty<DisciplinaDto>();
            AreasConhecimentos = areasConhecimentos ?? Enumerable.Empty<AreaDoConhecimentoDto>();
            GruposAreaOrdenacao = gruposAreaOrdenacao ?? Enumerable.Empty<ComponenteCurricularGrupoAreaOrdenacaoDto>();
            GrupoMatrizId = grupoMatrizId;
        }

        public IEnumerable<DisciplinaDto> ComponentesCurricularesTurma { get; private set; }
        public IEnumerable<AreaDoConhecimentoDto> AreasConhecimentos { get; private set; }
        public IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto> GruposAreaOrdenacao { get; private set; }
        public long GrupoMatrizId { get; private set; }
    }
}
