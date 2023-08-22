using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly IRepositorioCache repositorioCache;

        public RepositorioUsuario(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task Excluir(string usuarioRf, string connectionId)
        {
            var cacheUsuario = await Obter(usuarioRf);
            cacheUsuario.Remove(connectionId);

            if (cacheUsuario.Any())
                await repositorioCache.SalvarAsync(usuarioRf, cacheUsuario);
            else 
                await repositorioCache.RemoverAsync(usuarioRf);
        }

        public Task<List<string>> Obter(string usuarioRf)
            => repositorioCache.ObterAsync<List<string>>(usuarioRf, () => Task.FromResult(new List<string>()), "Obter Conexão Usuario");

        public async Task Salvar(string usuarioRf, string connectionId)
        {
            var cacheUsuario = await Obter(usuarioRf);

            cacheUsuario.Add(connectionId);

            await repositorioCache.SalvarAsync(usuarioRf, cacheUsuario);
        }
    }
}
