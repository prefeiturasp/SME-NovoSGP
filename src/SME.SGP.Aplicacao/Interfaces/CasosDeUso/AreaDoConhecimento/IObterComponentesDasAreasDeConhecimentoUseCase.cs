using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento
{
    public interface IObterComponentesDasAreasDeConhecimentoUseCase : IUseCase<(IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>), IEnumerable<DisciplinaDto>>
    {
    }
}
