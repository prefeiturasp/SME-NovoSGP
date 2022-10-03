using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public interface IRepositorioUsuario
    {
        Task<string> Obter(string usuarioRf);
        Task Salvar(string usuarioRf, string connectionId);
        Task Excluir(string usuarioRf);
    }
}
