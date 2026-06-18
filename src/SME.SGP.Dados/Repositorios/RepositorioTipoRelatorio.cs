using Dapper;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoRelatorio : IRepositorioTipoRelatorio
    {
        private readonly ISgpContext database;

        public RepositorioTipoRelatorio() { }

        public RepositorioTipoRelatorio(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<int> ObterTipoPorCodigo(string codigo)
        {
            var query = "select tipo_relatorio from relatorio_correlacao where codigo::text = @codigo";
            return await database.Conexao.QueryFirstAsync<int>(query, new { codigo });
        }
    }
}
