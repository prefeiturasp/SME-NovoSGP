using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase
    {
        Task Executar(int anoLetivo);
    }
}
