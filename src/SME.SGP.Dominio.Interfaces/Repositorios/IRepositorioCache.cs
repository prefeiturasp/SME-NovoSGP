using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCache
    {
        string Obter(string nomeChave);

        Task<string> ObterAsync(string nomeChave);

        Task RemoverAsync(string nomeChave);

        Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720);

        Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720);
    }
}