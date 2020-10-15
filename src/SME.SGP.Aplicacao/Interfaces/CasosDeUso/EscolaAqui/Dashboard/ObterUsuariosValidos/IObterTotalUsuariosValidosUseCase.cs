using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalUsuariosValidosUseCase
    {
        Task<long> Executar(string codigoDre, long codigoUe);
    }
}