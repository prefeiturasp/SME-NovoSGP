using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface ICalcularFrequenciaGeralUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagem);

    }
}
