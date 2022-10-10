using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public interface IRepositorioUsuario
    {
        Task<List<string>> Obter(string usuarioRf);
        Task Salvar(string usuarioRf, string connectionId);
        Task Excluir(string usuarioRf, string connectionId);
    }
}
