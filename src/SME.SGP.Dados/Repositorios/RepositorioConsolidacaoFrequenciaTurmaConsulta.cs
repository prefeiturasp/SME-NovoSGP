using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaTurmaConsulta : IRepositorioConsolidacaoFrequenciaTurmaConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioConsolidacaoFrequenciaTurmaConsulta(ISgpContextConsultas database)
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
	                     where quantidade_abaixo_minimo_frequencia >= 0
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
                            inner join registro_frequencia_aluno raa on raa.aula_id  = a.id and raa.codigo_aluno = afa.codigo_aluno and raa.valor = 2
                            inner join turma t on t.turma_id = a.turma_id 
                            inner join ue on ue.id = t.ue_id 
                            inner join dre on dre.id = ue.dre_id 
                        where not a.excluido and not afa.excluido and not raa.excluido
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

        public async Task<IEnumerable<FrequenciaGlobalMensalSemanalDto>> ObterFrequenciasConsolidadasPorTurmaMensalSemestral(int anoLetivo, long dreId, long ueId, int modalidade, string codigoTurma, DateTime dataInicioSemana, DateTime datafimSemana, int mes, int tipoPeriodoDashboard, bool visaoDre = false)
        {
            var query = new StringBuilder();

            query.AppendLine(@$"select t.nome NomeTurma,  
                                t.ano AnoTurma, 
                                t.modalidade_codigo ModalidadeTurma, 
                                dre.abreviacao AbreviacaoDre,
                                dre.dre_id CodigoDre,
                                cft.quantidade_acima_minimo_frequencia QuantidadeAcimaMinimoFrequencia, 
                                cft.quantidade_abaixo_minimo_frequencia QuantidadeAbaixoMinimoFrequencia,
                                {visaoDre.ToString().ToLower()} as VisaoDre::boolean, 
                                {ueId.ToString()} as ueId::bigint 
                                from consolidacao_frequencia_turma cft
                                inner join turma t on cft.turma_id = t.id
                                inner join ue on t.ue_id = ue.ue_id
                                inner join dre on dre.id = ue.ue_id
                         where ano_letivo = @anoLetivo
                           and modalidade_codigo = @modalidade
                           and tipo = @tipoPeriodo ");

            if (!string.IsNullOrEmpty(codigoTurma) && !codigoTurma.Contains("-99"))
                query.AppendLine("and t.turma_id = @codigoTurma ");

            if (dreId != -99)
                query.AppendLine("and dre.id = @dreId ");

            if (ueId != -99)
                query.AppendLine("and ue.id = @ueId ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine(@"and periodo_inicio = @dataInicioSemana and periodo_fim = @datafimSemana  ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Mensal)
                query.AppendLine(@"and Extract('Month' From periodo_inicio) = @mes ");

            if (visaoDre)
                query.AppendLine("group by dre_abreviacao, tipo, dre_codigo ");
            else if (ueId == -99)
                query.AppendLine("group by turma_ano, tipo, dre_codigo");
            else if (ueId != -99 && !visaoDre)
                query.AppendLine("group by turma_nome, tipo, dre_codigo");

            var frequencias = await database.Conexao.QueryAsync<FrequenciaGlobalMensalSemanalDto>(query.ToString(), new
            {
                dreId,
                ueId,
                anoLetivo,
                modalidade,
                dataInicioSemana,
                datafimSemana,
                mes,
                tipoPeriodo = tipoPeriodoDashboard
            });

            return frequencias.OrderBy(f => f.Descricao).ThenBy(f => f.CodigoDre).ToList();
        }
    }
}