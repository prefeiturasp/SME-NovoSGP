using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoFinal : RepositorioBase<FechamentoFinal>, IRepositorioFechamentoFinal
    {
        public RepositorioFechamentoFinal(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<FechamentoFinal>> ObterPorFiltros(string turmaCodigo, string[] disciplinasCodigo)
        {
            var query = new StringBuilder(@"select
	                            fn.*
                            from
	                            fechamento_final fn
                            inner join turma t on
	                            fn.turma_id = t.id
                            where 1=1 ");

            if (!string.IsNullOrEmpty(turmaCodigo))
                query.AppendLine("and t.turma_id = @turmaCodigo");

            if (disciplinasCodigo != null && disciplinasCodigo.Length > 0)
                query.AppendLine("and fn.disciplina_codigo = ANY(@disciplinasCodigo)");

            return await database.Conexao.QueryAsync<FechamentoFinal>(query.ToString(), new { turmaCodigo, disciplinasCodigo });
        }
    }
}