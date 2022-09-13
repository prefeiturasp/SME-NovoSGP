using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Worker
{

    public interface INotificacaoSgpHub
    {
        Task Criada(MensagemRabbit mensagem);
        Task Lida(MensagemRabbit mensagem);
        Task Excluida(MensagemRabbit mensagem);
    }
}
