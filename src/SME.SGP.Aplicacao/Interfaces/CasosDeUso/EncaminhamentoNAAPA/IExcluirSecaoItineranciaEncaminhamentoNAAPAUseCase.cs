using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase 
    {
        Task<bool> Executar(long encaminhamentoNAAPAId, long secaoItineranciaId);
    }
}
