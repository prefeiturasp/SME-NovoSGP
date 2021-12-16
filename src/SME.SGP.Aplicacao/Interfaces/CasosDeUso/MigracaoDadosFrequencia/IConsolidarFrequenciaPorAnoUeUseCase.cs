using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsolidarFrequenciaPorAnoUeUseCase
    {
        Task<bool> Executar(int anoLetivo, long ueId);
    }
}
