using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IExcluirNotificacaoDiarioBordoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}