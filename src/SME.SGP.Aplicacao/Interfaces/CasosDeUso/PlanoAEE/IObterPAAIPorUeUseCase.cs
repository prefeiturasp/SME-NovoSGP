using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPAAIPorUeUseCase : IUseCase<string, IEnumerable<SupervisorDto>>
    {
        Task<IEnumerable<SupervisorDto>> Executar(string codigoUe);
    }
}
