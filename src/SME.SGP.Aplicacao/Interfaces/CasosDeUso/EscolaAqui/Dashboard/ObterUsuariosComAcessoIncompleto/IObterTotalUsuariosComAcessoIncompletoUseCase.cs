using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalUsuariosComAcessoIncompletoUseCase
    {
        Task<long> Executar(string codigoDre, long codigoUe);
    }
}