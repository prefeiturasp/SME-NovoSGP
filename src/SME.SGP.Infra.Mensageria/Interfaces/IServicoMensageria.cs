using System.Threading.Tasks;

namespace SME.SGP.Infra.Interface
{
    public interface IServicoMensageria<T>
        where T : class
    {
        Task<bool> Publicar(T mensagem, string rota, string exchange, string nomeAcao);
    }

    public interface IServicoMensageriaSGP : IServicoMensageria<MensagemRabbit> { }
    public interface IServicoMensageriaLogs : IServicoMensageria<LogMensagem> { }
}
