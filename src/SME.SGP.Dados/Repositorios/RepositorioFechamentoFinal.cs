using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoFinal : RepositorioBase<FechamentoFinal>, IRepositorioFechamentoFinal
    {
        public RepositorioFechamentoFinal(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<FechamentoFinal>> ObterPorFiltros(string turmaCodigo)
        {
            var query = @"select
	                            fn.*
                            from
	                            fechamento_final fn
                            inner join turma t on
	                            fn.turma_id = t.id
                            where
	                            t.turma_id = @turmaCodigo";

            return await database.Conexao.QueryAsync<FechamentoFinal>(query, new { turmaCodigo });
        }
    }
}