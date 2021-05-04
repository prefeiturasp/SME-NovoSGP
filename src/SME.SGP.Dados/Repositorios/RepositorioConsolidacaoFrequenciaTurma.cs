using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaTurma : IRepositorioConsolidacaoFrequenciaTurma
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoFrequenciaTurma(ISgpContext database)
        {
            this.database = database;
        }

        private string ObterWhereFrequenciaGlobalPorAno(long dreId, long ueId, Modalidade? modalidade)
        {
            var subQuery = "";
            if (dreId > 0) subQuery += " and cft.dre_id = @dreId";
            if (ueId > 0) subQuery += " and cft.ue_id = @ueId";
            if (modalidade > 0) subQuery += " and cft.modalidade  = @modalidade";
            return subQuery;
        }

        public async Task<IEnumerable<FrequenciaGlobalPorAnoDto>> ObterFrequenciaGlobalPorAnoAsync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade)
        {
            var sql = $@"select * from (
	                        select cft.modalidade, 
		                           t.ano, 
		                           cft.quantidade_acima_minimo_frequencia as quantidade , 
		                           'Qtd. acima do mínimo de frequencia' as descricao
	                          from consolidacao_frequencia_turma cft
	                         inner join turma t on t.id = cft.turma_id
	                         where quantidade_acima_minimo_frequencia > 0
	                           and t.ano_letivo = @anoLetivo
                              {ObterWhereFrequenciaGlobalPorAno(dreId, ueId, modalidade)}
                             union 
                     	    select cft.modalidade, 
		                           t.ano, 
		                           cft.quantidade_abaixo_minimo_frequencia as quantidade, 
		                           'Qtd. abaixo do mínimo de frequencia' as descricao
	                          from consolidacao_frequencia_turma cft
	                         inner join turma t on t.id = cft.turma_id
	                         where quantidade_abaixo_minimo_frequencia > 0
	                           and t.ano_letivo = @anoLetivo
                                {ObterWhereFrequenciaGlobalPorAno(dreId, ueId, modalidade)}
                            ) x
                        order by x.modalidade, x.ano, x.quantidade";

            return await database
                .Conexao
                .QueryAsync<FrequenciaGlobalPorAnoDto>(sql, new { modalidade, dreId, ueId, anoLetivo });
        }

        public async Task<bool> ExisteConsolidacaoFrequenciaTurmaPorAno(int ano)
        {
            var query = @"select 1 
                          from consolidacao_frequencia_turma c
                         inner join turma t on t.id = c.turma_id
                         where t.ano_letivo = @ano";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { ano });
        }

        public async Task<long> Inserir(ConsolidacaoFrequenciaTurma consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoFrequenciasTurmasPorAno(int ano)
        {
            var query = @"delete from consolidacao_frequencia_turma
                        where turma_id in (
                            select id from turma where ano_letivo = @ano)";

            await database.Conexao.ExecuteScalarAsync(query, new { ano });
        }
    }
}