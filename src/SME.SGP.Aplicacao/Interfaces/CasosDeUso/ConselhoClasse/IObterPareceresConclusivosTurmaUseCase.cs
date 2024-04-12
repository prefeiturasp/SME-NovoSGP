using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterPareceresConclusivosTurmaUseCase : IUseCase<long, IEnumerable<ParecerConclusivoDto>>
    {
    }
}
