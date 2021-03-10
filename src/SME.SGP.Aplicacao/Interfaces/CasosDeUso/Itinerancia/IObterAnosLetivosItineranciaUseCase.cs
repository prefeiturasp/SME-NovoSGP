using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAnosLetivosItineranciaUseCase
    {
        Task<IEnumerable<int>> Executar();
    }
}