using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub.Interface
{
    public interface IEventosNotificacao
    {
        Task Criada(MensagemCriacaoNotificacaoDto mensagem);
        Task Lida(MensagemLeituraNotificacaoDto mensagem);
        Task Excluida(MensagemExclusaoNotificacaoDto mensagem);
    }
}
