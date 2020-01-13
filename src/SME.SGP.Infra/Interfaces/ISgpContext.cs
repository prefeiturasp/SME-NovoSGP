
using System.Data;
using System.Data.Common;

namespace SME.SGP.Infra
{
    public interface ISgpContext : IDbConnection
    {
        IDbConnection Conexao { get; }
        string UsuarioLogado { get; }
        string UsuarioLogadoNomeCompleto { get; }
        string UsuarioLogadoRF { get; }
    }
}