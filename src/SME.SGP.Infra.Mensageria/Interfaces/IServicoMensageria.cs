using System.Threading.Tasks;

namespace SME.SGP.Infra.Interface
{
    public interface IServicoMensageria
    {
        Task<bool> Publicar(MensagemRabbit mensagem, string rota, string exchange, string nomeAcao);
        Task<bool> Publicar<T>(T mensagem, string rota, string exchange, string nomeAcao);
    }

    public interface IServicoMensageriaSGP : IServicoMensageria { }
    public interface IServicoMensageriaLogs : IServicoMensageria { }
}
