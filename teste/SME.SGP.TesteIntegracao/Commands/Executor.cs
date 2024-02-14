using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Commands
{
    public class Executor : IExecutor
    {
        private IComando Comando { get; set; }
        public Task ExecutarComando()
        => Comando.Executar();

        public void SetarComando(IComando comando)
        {
            this.Comando = comando;
        }
    }
}
