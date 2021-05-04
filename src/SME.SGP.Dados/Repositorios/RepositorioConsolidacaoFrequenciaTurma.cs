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

        public async Task<IEnumerable<FrequenciaGlobalPorAnoDto>> ObterFrequenciaGlobalPorAnoAsync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, int semestre)
        {
            string campoNomeTurma = ueId>0? "t.nome as nomeTurma,": ""; 
            string agrupamentoTurma = ueId > 0 ? "t.nome," : "";

            var sql = $@"select t.modalidade_codigo as modalidade, 
                                {campoNomeTurma}
		                        t.ano,
		                        sum(cft.quantidade_acima_minimo_frequencia)  AS QuantidadeAcimaMinimoFrequencia,
		                        sum(cft.quantidade_abaixo_minimo_frequencia) AS QuantidadeAbaixoMinimoFrequencia
	                      from consolidacao_frequencia_turma cft
	                     inner join turma t on t.id = cft.turma_id
                         inner join ue on ue.id = t.ue_id 
                         inner join dre on dre.id = ue.dre_id 
	                     where quantidade_abaixo_minimo_frequencia > 0
	                       and t.ano_letivo = @anoLetivo
                           and t.modalidade_codigo = @modalidade";
            if (semestre > 0) sql += @"  and t.semestre = @semestre";
            if (dreId > 0) sql += @" and dre.id = @dreId";
            if(ueId > 0) sql += @"  and ue.id = @ueId";
            sql += $@"    group by t.modalidade_codigo,{agrupamentoTurma} t.ano 
                         order by t.modalidade_codigo,{agrupamentoTurma} t.ano";

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