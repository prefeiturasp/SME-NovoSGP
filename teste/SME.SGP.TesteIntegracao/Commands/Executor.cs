using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Commands
{
    public abstract class Executor
    {
        public static Task ExecutarComando(IComando comando)
            => comando.Executar();
    }
}
