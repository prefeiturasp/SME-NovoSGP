using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterComponentesDasAreasDeConhecimentoUseCase : IUseCase<(IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>), IEnumerable<DisciplinaDto>>
    {
    }
}
