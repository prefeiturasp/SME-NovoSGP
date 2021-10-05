using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<IEnumerable<FrequenciaGlobalPorAnoDto>> ObterFrequenciaGlobalPorAnoAsync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, int semestre)
        {
            string campoNomeTurma = ueId > 0 ? "t.nome as nomeTurma," : "";
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
            if (ueId > 0) sql += @"  and ue.id = @ueId";
            sql += $@"    group by t.modalidade_codigo,{agrupamentoTurma} t.ano 
                         order by t.modalidade_codigo,{agrupamentoTurma} t.ano";

            return await database
                .Conexao
                .QueryAsync<FrequenciaGlobalPorAnoDto>(sql, new { modalidade, dreId, ueId, anoLetivo });
        }

        public async Task<IEnumerable<FrequenciaGlobalPorDreDto>> ObterFrequenciaGlobalPorDreAsync(int anoLetivo, Modalidade modalidade, string ano, int? semestre)
        {
            const string sqlBase = @"
                SELECT
                    dre.abreviacao AS Dre,
                    dre.dre_id,
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
                    t.ano_letivo = @anoLetivo
                    AND t.modalidade_codigo = @modalidade ";

            var sql = new StringBuilder(sqlBase);

            if (!string.IsNullOrWhiteSpace(ano))
            {
                sql.AppendLine(" AND t.ano = @ano ");
            }

            if (modalidade == Modalidade.EJA && semestre.HasValue)
            {
                sql.AppendLine(" AND t.semestre = @semestre ");
            }

            sql.AppendLine(@" GROUP BY dre.abreviacao, dre.dre_id
                              ORDER BY dre.dre_id");

            return await database
                .Conexao
                .QueryAsync<FrequenciaGlobalPorDreDto>(sql.ToString(), new { anoLetivo, modalidade, ano, semestre });
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

        private string ObterWhereAusenciasComJustificativaASync(long dreId, long ueId, Modalidade? modalidade, int semestre)
        {
            var subQuery = "";
            if (dreId > 0) subQuery += " and dre.id = @dreId";
            if (ueId > 0) subQuery += " and ue.id = @ueId";
            if (modalidade > 0) subQuery += " and t.modalidade_codigo  = @modalidade";
            if (semestre >= 0) subQuery += " and t.semestre   = @semestre";
            return subQuery;
        }

        public async Task<IEnumerable<GraficoAusenciasComJustificativaDto>> ObterAusenciasComJustificativaASync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, int semestre)
        {
            string campoNomeTurma = ueId > 0 ? "t.nome as nomeTurma," : "";
            string agrupamentoTurma = ueId > 0 ? "t.nome," : "";

            var sql = $@"select count(afa.id) as quantidade, 
                                {campoNomeTurma}
                                t.modalidade_codigo as modalidade, 
                                t.ano 
                        from anotacao_frequencia_aluno afa 
                            inner join aula a on a.id = afa.aula_id 
                            inner join registro_frequencia rf on rf.aula_id = a.id 
                            inner join registro_frequencia_aluno raa on raa.registro_frequencia_id = rf.id and raa.codigo_aluno = afa.codigo_aluno and raa.valor = 2
                            inner join turma t on t.turma_id = a.turma_id 
                            inner join ue on ue.id = t.ue_id 
                            inner join dre on dre.id = ue.dre_id 
                        where not a.excluido and not rf.excluido and not afa.excluido and not raa.excluido
                          and (motivo_ausencia_id is not null or anotacao is not null)
                          and afa.excluido = false 
                          and t.ano_letivo = @anoLetivo
                        {ObterWhereAusenciasComJustificativaASync(dreId, ueId, modalidade, semestre)}
                        group by {agrupamentoTurma} t.modalidade_codigo, t.ano
                        order by {agrupamentoTurma} t.modalidade_codigo, t.ano
    ";

            return await database
                .Conexao
                .QueryAsync<GraficoAusenciasComJustificativaDto>(sql, new { modalidade, dreId, ueId, anoLetivo, semestre });
        }

        public async Task<long> InserirConsolidacaoDashBoard(ConsolidacaoDashBoardFrequencia consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }
        public async Task ExcluirConsolidacaoDashBoard(int anoLetivo, long turmaId, DateTime dataAula, DateTime? dataInicioSemanda, DateTime? dataFinalSemena, int? mes, TipoPeriodoDashboardFrequencia tipoPeriodo)
        {
            var query = new StringBuilder(@"delete 
                                              from consolidado_dashboard_frequencia
                                             where turma_id = @turmaId
                                               and tipo = @tipoPeriodo
                                               and ano_letivo = @anoLetivo ");

            if(tipoPeriodo == TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine("and data_aula::date = @dataAula::date ");

            if (tipoPeriodo == TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine(@"and data_inicio::date = @dataInicioSemanda::date
                                   and data_fim::date = @dataFinalSemena::date ");

            if (tipoPeriodo == TipoPeriodoDashboardFrequencia.Mensal)
                query.AppendLine("and mes = @mes ");

            var parametros = new
            {
                anoLetivo,
                turmaId,
                dataAula,
                dataInicioSemanda,
                dataFinalSemena,
                mes,
                tipoPeriodo
            };

            try
            {
                await database.Conexao.ExecuteScalarAsync(query.ToString(), parametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}