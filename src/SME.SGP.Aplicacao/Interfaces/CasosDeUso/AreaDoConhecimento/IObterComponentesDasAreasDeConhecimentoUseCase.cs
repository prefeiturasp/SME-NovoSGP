using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterComponentesDasAreasDeConhecimentoUseCase : IUseCase<(IEnumerable<DisciplinaDto> Disciplinas, IEnumerable<AreaDoConhecimentoDto> AreasConhecimento), IEnumerable<DisciplinaDto>>
    {
    }
}
