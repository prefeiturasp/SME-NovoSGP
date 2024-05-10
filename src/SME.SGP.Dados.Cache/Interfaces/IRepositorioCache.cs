using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCache
    {
        string Obter(string nomeChave, bool utilizarGZip = false);        
        string Obter(string nomeChave, string telemetriaNome, bool utilizarGZip = false);        
        Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false);
        Task<string> ObterAsync(string nomeChave, string telemetriaNome, bool utilizarGZip = false);
        Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false);
        Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, string telemetriaNome, int minutosParaExpirar = 720, bool utilizarGZip = false);

        Task<T> ObterObjetoAsync<T>(string nomeChave, bool utilizarGZip = false)
            where T : new();

        Task<T> ObterObjetoAsync<T>(string nomeChave, string telemetriaNome, bool utilizarGZip = false) where T : new();
        
        Task RemoverAsync(string nomeChave);
        Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false);
        Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false);
    }
}