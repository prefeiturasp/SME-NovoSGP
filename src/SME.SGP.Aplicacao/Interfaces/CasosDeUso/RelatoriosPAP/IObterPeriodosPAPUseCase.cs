using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterPeriodosPAPUseCase : IUseCase<string, IEnumerable<PeriodosPAPDto>>
    {
    }
}
