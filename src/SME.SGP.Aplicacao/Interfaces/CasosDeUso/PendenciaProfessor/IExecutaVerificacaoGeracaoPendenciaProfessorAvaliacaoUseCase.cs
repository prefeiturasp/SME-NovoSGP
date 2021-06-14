using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagem);
    }
}
