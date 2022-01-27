using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IReprocessarParecerConclusivoPorAnoUseCase
    {
        Task Executar(int anoLetivo);
    }
}
