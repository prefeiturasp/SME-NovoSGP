using Npgsql;
using System.Data;

namespace SME.SGP.Dados.Contexto
{
    public interface SgpContext : IDbConnection
    {
        NpgsqlConnection Connection();
    }
}