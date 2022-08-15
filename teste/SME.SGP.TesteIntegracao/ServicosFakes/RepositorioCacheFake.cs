using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class RepositorioCacheFake : IRepositorioCache
    {
        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            return string.Empty;
        }

        public Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            return buscarDados();
        }

        public Task RemoverAsync(string nomeChave)
        {
            return Task.FromResult(string.Empty);
        }

        public void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {}

        public Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            return Task.FromResult(string.Empty);
        }

        public Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            return Task.FromResult(string.Empty);
        }
    }
}