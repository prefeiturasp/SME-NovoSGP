using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutaConsolidacaoFrequenciaPorAnoUseCase
    {
        Task Executar(int ano);
    }
}
