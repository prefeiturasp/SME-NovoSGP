using SME.SGP.Dominio.Interfaces;
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

        public Task Excluir(string usuarioRf)
            => repositorioCache.RemoverAsync(usuarioRf);

        public Task<string> Obter(string usuarioRf)
            => repositorioCache.ObterAsync(usuarioRf, "Obter Conexão Usuario");

        public Task Salvar(string usuarioRf, string connectionId)
            => repositorioCache.SalvarAsync(usuarioRf, connectionId);
    }
}
