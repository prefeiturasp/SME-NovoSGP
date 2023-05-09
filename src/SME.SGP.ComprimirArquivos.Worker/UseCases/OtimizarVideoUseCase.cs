using System;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class OtimizarVideoUseCase : IOtimizarVideoUseCase
    {
        public Task<bool> Executar(MensagemRabbit mensagem)
        {
            throw new NotImplementedException();
        }
    }
}
