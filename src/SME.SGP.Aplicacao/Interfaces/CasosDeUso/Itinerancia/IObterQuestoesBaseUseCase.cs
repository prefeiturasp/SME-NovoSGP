using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterQuestoesBaseUseCase
    {
        Task<ItineranciaQuestoesBaseDto> Executar();
    }
}