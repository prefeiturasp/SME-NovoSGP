using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExcluirSecaoItineranciaAtendimentoNAAPAUseCase 
    {
        Task<bool> Executar(long encaminhamentoNAAPAId, long secaoItineranciaId);
    }
}
