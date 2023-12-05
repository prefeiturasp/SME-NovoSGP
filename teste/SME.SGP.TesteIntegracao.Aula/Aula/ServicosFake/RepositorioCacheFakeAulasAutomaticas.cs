using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.Aula.ServicosFake
{
    public class RepositorioCacheFakeAulasAutomaticas : IRepositorioCache
    {
        private readonly IDictionary<string, object> objetosCache;

        public RepositorioCacheFakeAulasAutomaticas()
        {
            objetosCache = new Dictionary<string, object>();
        }

        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            throw new NotImplementedException();
        }

        public string Obter(string nomeChave, string telemetriaNome, bool utilizarGZip = false)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false)
        {
            var objeto = objetosCache[nomeChave.Replace("\"", string.Empty)];

            return await Task.FromResult(objetosCache != null ? JsonConvert.SerializeObject(objeto) : null);
        }

        public Task<string> ObterAsync(string nomeChave, string telemetriaNome, bool utilizarGZip = false)
        {
            throw new NotImplementedException();
        }

        public Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            return buscarDados();
        }

        public Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, string telemetriaNome, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            throw new NotImplementedException();
        }

        public Task<T> ObterObjetoAsync<T>(string nomeChave, bool utilizarGZip = false) where T : new()
        {
            throw new NotImplementedException();
        }

        public Task<T> ObterObjetoAsync<T>(string nomeChave, string telemetriaNome, bool utilizarGZip = false) where T : new()
        {
            throw new NotImplementedException();
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var nomeChaveSemAspas = nomeChave.Replace("\"", string.Empty);
            if (objetosCache.ContainsKey(nomeChaveSemAspas))
                objetosCache.Remove(nomeChaveSemAspas);

            await Task.CompletedTask;
        }

        public Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            throw new NotImplementedException();
        }

        public async Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var nomeChaveSemAspas = nomeChave.Replace("\"", string.Empty);
            if (objetosCache.ContainsKey(nomeChaveSemAspas))
                objetosCache[nomeChaveSemAspas] = valor;
            else
                objetosCache.Add(nomeChaveSemAspas, valor);

            await Task.CompletedTask;
        }
    }
}
