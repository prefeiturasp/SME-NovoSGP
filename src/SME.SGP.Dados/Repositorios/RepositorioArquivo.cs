using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioArquivo : RepositorioBase<Arquivo>, IRepositorioArquivo
    {
        public RepositorioArquivo(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<Arquivo> ObterPorCodigo(Guid codigo)
        {
            const string query = @"select * 
                                    from arquivo
                                    where codigo = @codigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<Arquivo>(query, new { codigo });
        }

        public async Task<IEnumerable<Arquivo>> ObterPorCodigos(Guid[] codigos)
        {
            const string query = @"select * 
                                    from arquivo
                                    where codigo = ANY(@codigos)";

            return await database.Conexao.QueryAsync<Arquivo>(query, new { codigos });
        }

        public async Task<IEnumerable<Arquivo>> ObterPorIds(long[] ids)
        {
            const string query = @"select * 
                                    from arquivo
                                    where id = ANY(@ids)";

            return await database.Conexao.QueryAsync<Arquivo>(query, new { ids });
        }

        public async Task<bool> ExcluirArquivoPorCodigo(Guid codigoArquivo)
        {
            var query = "delete from Arquivo where codigo = @codigoArquivo";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { codigoArquivo });
        }

        public async Task<bool> ExcluirArquivoPorId(long id)
        {
            const string query = "delete from Arquivo where id = @id";
            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { id });
        }
        
        public async Task<bool> ExcluirArquivosPorIds(long[] ids)
        {
            const string query = "delete from Arquivo where id = ANY(@ids)";
            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { ids });
        }

        public async Task<long> ObterIdPorCodigo(Guid arquivoCodigo)
        {
            var query = @"select id
                            from arquivo 
                           where codigo = @arquivoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { arquivoCodigo });
        }

        public async Task<IEnumerable<Arquivo>> ObterComprimir(DateTime dataInicio, DateTime dataFim)
        {
            const string query = @"select * 
                                    from arquivo
                                    where criado_em > @dataInicio
                                        and criado_em < @dataFim
                                        and tipo_conteudo <> 'application/pdf'
                                    order by criado_em desc";

            return await database.Conexao.QueryAsync<Arquivo>(query, new { dataInicio, dataFim });
        }
    }
}
