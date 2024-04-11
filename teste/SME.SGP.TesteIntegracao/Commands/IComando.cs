
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Commands
{
    public interface IComando
    {
        Task Executar();
    }
}
