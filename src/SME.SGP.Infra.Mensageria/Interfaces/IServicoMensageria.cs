using RabbitMQ.Client;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Interface
{
    public interface IServicoMensageria<T>
        where T : class
    {
        Task<bool> Publicar(T mensagem, string rota, string exchange, string nomeAcao, IModel canalRabbit = null);
    }

    public interface IServicoMensageriaSGP : IServicoMensageria<MensagemRabbit> { }
    public interface IServicoMensageriaLogs : IServicoMensageria<LogMensagem> { }
    public interface IServicoMensageriaMetricas : IServicoMensageria<MetricaMensageria> 
    {
        Task Publicado(string rota);
        Task Obtido(string rota);
        Task Concluido(string rota);
        Task Erro(string rota);
    }
}
