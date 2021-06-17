using System;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioArquivo : RepositorioBase<Arquivo>, IRepositorioArquivo
    {
        public RepositorioArquivo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<Arquivo> ObterPorCodigo(Guid codigo)
        {
            var query = @"select * 
                            from arquivo
                           where codigo = @codigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<Arquivo>(query, new { codigo });
        }

        public async Task<bool> ExcluirArquivoPorCodigo(Guid codigoArquivo)
        {
            var query = "delete from Arquivo where codigo = @codigoArquivo";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { codigoArquivo });
        }

        public async Task<bool> ExcluirArquivoPorId(long id)
        {
            var query = "delete from Arquivo where id = @id";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }

        public async Task<long> ObterIdPorCodigo(Guid arquivoCodigo)
        {
            var query = @"select id
                            from arquivo 
                           where codigo = @arquivoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { arquivoCodigo });
        }
    }
}
