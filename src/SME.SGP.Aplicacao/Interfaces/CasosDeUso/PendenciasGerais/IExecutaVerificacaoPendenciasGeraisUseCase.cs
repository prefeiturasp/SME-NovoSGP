using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExecutaVerificacaoPendenciasGeraisUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagem);
    }
}
