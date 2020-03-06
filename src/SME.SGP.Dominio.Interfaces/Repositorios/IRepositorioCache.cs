using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCache
    {
        string Obter(string nomeChave);

        T Obter<T>(string nomeChave);

        T Obter<T>(string nomeChave, Func<T> buscarDados, int minutosParaExpirar);

        Task<string> ObterAsync(string nomeChave);

        Task RemoverAsync(string nomeChave);

        Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720);
    }
}