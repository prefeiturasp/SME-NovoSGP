using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IMapearAreasDoConhecimentoUseCase : IUseCase<(IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>, long), IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>>
    {
    }
}
