using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IMapearAreasDoConhecimentoUseCase : IUseCase<(IEnumerable<DisciplinaDto> Disciplinas, IEnumerable<AreaDoConhecimentoDto> AreasConhecimento,
                                                                   IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto> ComponentesCuricularesGrupoAreaOrdem, long GrupoMatrizId), 
                                                                   IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>>
    {
    }
}
