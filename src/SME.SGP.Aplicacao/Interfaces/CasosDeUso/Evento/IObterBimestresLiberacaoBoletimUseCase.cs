using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterBimestresLiberacaoBoletimUseCase
    {
        Task<int[]> Executar(string codigoTurma);
    }
}
