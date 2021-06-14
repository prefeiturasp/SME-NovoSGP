using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICarregarDadosAulasFrequenciaUseCase
    {
        Task<bool> Executar(int[] anosLetivos);
    }
}
