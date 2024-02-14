
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Commands
{
    public interface IExecutor
    {
        void SetarComando(IComando comando);
        Task ExecutarComando();
    }
}
