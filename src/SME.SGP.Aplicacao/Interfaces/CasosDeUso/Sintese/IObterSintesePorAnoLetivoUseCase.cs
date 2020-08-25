using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterSintesePorAnoLetivoUseCase : IUseCase<int, IEnumerable<SinteseDto>>
    {
    }
}
