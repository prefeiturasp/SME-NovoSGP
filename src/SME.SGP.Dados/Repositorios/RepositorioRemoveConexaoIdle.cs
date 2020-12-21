using Npgsql;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRemoveConexaoIdle : IRepositorioRemoveConexaoIdle
    {
        public RepositorioRemoveConexaoIdle()
        {

        }

        public void RemoveConexaoIdle()
        {
            NpgsqlConnection.ClearAllPools();
        }
    }
}
