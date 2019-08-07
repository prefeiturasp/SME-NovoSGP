using Npgsql;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public interface ISgpContext : IDbConnection
    {
        NpgsqlConnection Conexao();
    }
}