using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterOrdenacaoAreasConhecimentoUseCase : IUseCase<(IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>), IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>
    {
    }
}
