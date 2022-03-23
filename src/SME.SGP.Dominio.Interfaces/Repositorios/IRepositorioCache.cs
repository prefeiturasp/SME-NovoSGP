using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCache
    {
        string Obter(string nomeChave, bool utilizarGZip = false);        

        Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false);

        Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false);

        Task RemoverAsync(string nomeChave);

        void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false);

        Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false);

        Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false);
    }
}