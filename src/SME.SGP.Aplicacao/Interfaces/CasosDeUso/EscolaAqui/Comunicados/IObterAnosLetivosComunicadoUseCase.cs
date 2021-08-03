using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAnosLetivosComunicadoUseCase
    {
        Task<IEnumerable<int>> Executar();
    }
}
