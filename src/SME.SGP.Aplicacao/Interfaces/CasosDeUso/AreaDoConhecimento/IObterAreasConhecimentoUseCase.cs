using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.AreaDoConhecimento
{
    public interface IObterAreasConhecimentoUseCase : IUseCase<IEnumerable<DisciplinaDto>, IEnumerable<AreaDoConhecimentoDto>>
    {
    }
}
