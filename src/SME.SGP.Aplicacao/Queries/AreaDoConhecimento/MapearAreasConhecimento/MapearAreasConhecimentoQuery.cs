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
            ComponentesCurricularesTurma = componentesCurricularesTurma;
            AreasConhecimentos = areasConhecimentos;
            GruposAreaOrdenacao = gruposAreaOrdenacao;
            GrupoMatrizId = grupoMatrizId;
        }

        public IEnumerable<DisciplinaDto> ComponentesCurricularesTurma { get; set; }
        public IEnumerable<AreaDoConhecimentoDto> AreasConhecimentos { get; set; }
        public IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto> GruposAreaOrdenacao { get; set; }
        public long GrupoMatrizId { get; set; }
    }
}
