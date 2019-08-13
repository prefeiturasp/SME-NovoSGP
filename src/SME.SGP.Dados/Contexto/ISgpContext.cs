using Npgsql;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public interface ISgpContext : IDbConnection
    {
        string UsuarioLogado { get; }

        NpgsqlConnection Conexao();
    }
}