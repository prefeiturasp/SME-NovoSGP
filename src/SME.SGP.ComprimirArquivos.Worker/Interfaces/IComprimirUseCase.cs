
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public interface IComprimirUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagem);
    }
}
