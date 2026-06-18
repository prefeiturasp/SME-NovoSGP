
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public interface IComprimirUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagem);
    }
}
