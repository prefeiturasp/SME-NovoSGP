using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterRfsPorNomesItineranciaUseCase
    {
        Task<IEnumerable<ItineranciaNomeRfCriadorRetornoDto>> Executar(string nomeParaBusca);
    }
}
