using System;
using System.Threading.Tasks;
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
    }
}
