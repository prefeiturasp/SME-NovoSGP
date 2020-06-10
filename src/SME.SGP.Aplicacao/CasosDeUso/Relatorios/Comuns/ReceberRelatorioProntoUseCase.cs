using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{


    public class ReceberRelatorioProntoUseCase : IReceberRelatorioProntoUseCase
    {
        public Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            return Task.FromResult(true);
        }
    }
}
