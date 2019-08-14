using Npgsql;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public interface ISgpContext : IDbConnection
    {
        NpgsqlConnection Conexao { get; }
        string UsuarioLogado { get; }
        string UsuarioLogadoNomeCompleto { get; }
        string UsuarioLogadoRF { get; }
    }
}