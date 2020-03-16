using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCache
    {
        string Obter(string nomeChave);

        T Obter<T>(string nomeChave);

        Task<T> Obter<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720);

        Task<string> ObterAsync(string nomeChave);

        Task RemoverAsync(string nomeChave);

        void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720);

        Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720);
    }
}