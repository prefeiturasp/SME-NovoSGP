using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCache
    {
        string Obter(string nomeChave);

        Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720);
    }
}