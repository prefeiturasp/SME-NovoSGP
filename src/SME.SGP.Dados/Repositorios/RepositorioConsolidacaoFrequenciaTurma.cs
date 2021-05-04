using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaTurma : IRepositorioConsolidacaoFrequenciaTurma
    {
        protected readonly ISgpContext database;

        public RepositorioConsolidacaoFrequenciaTurma(ISgpContext database)
        {
            this.database = database;
        }

        private string ObterWhereFrequenciaGlobalPorAno(long dreId, long ueId, Modalidade? modalidade)
        {
            var subQuery = "";
            if (dreId > 0) subQuery += " and dre.id = @dreId";
            if (ueId > 0) subQuery += " and ue.id = @ueId";
            if (modalidade > 0) subQuery += " and t.modalidade_codigo  = @modalidade";
            return subQuery;
        }

        public async Task<IEnumerable<FrequenciaGlobalPorAnoDto>> ObterFrequenciaGlobalPorAnoAsync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade)
        {
            var sql = $@"select * from (
	                        select t.modalidade_codigo as modalidade, 
		                           t.ano, 
		                           cft.quantidade_acima_minimo_frequencia as quantidade , 
		                           'Qtd. acima do mínimo de frequencia' as descricao
	                          from consolidacao_frequencia_turma cft
	                         inner join turma t on t.id = cft.turma_id
                             inner join ue on ue.id = t.ue_id 
                             inner join dre on dre.id = ue.dre_id 
	                         where quantidade_acima_minimo_frequencia > 0
	                           and t.ano_letivo = @anoLetivo
                              {ObterWhereFrequenciaGlobalPorAno(dreId, ueId, modalidade)}
                             union 
                     	    select t.modalidade_codigo as modalidade, 
		                           t.ano, 
		                           cft.quantidade_abaixo_minimo_frequencia as quantidade, 
		                           'Qtd. abaixo do mínimo de frequencia' as descricao
	                          from consolidacao_frequencia_turma cft
	                         inner join turma t on t.id = cft.turma_id
                             inner join ue on ue.id = t.ue_id 
                             inner join dre on dre.id = ue.dre_id 
	                         where quantidade_abaixo_minimo_frequencia > 0
	                           and t.ano_letivo = @anoLetivo
                                {ObterWhereFrequenciaGlobalPorAno(dreId, ueId, modalidade)}
                            ) x
                        order by x.modalidade, x.ano, x.quantidade";

            return await database
                .Conexao
                .QueryAsync<FrequenciaGlobalPorAnoDto>(sql, new { modalidade, dreId, ueId, anoLetivo });
        }

        public async Task<IEnumerable<FrequenciaGlobalPorDreDto>> ObterFrequenciaGlobalPorDreAsync(int anoLetivo)
        {
            const string sql = @"
                SELECT
                    dre.abreviacao,
                    SUM(cft.quantidade_acima_minimo_frequencia) AS QuantidadeAcimaMinimoFrequencia,
                    SUM(cft.quantidade_abaixo_minimo_frequencia) AS QuantidadeAbaixoMinimoFrequencia
                FROM
                    consolidacao_frequencia_turma cft 
                INNER JOIN
                    turma t 
                    ON t.id = cft.turma_id
                INNER JOIN
                    ue 
                    ON ue.id = t.ue_id 
                INNER JOIN 
                    dre 
                    ON dre.id = ue.dre_id
                WHERE
                    t.ano = @anoLetivo
                GROUP BY
                    dre.abreviacao";

            return await database
                .Conexao
                .QueryAsync<FrequenciaGlobalPorDreDto>(sql, new { anoLetivo });
        }
    }
}