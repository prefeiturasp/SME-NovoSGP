using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterPAAIPorUeUseCase : IUseCase<string, IEnumerable<ServidorDto>>
    {
    }
}
