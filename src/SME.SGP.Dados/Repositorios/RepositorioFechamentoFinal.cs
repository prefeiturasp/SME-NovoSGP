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

        public async Task<IEnumerable<FechamentoFinal>> ObterPorFiltros(string turmaCodigo, string componenteCurricularCodigo)
        {
            var query = @"select * from compensacao_ausencia_disciplina_regencia where not excluido and compensacao_ausencia_id = @compensacaoId";

            return await database.Conexao.QueryAsync<FechamentoFinal>(query, new { turmaCodigo, componenteCurricularCodigo });
        }
    }
}