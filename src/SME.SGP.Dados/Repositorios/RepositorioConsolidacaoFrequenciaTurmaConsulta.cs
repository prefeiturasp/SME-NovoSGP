using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
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
                           and cft.tipo_consolidacao = @tipoConsolidacao
                           and t.ano_letivo = @anoLetivo
                           and t.modalidade_codigo = @modalidade";
            if (semestre > 0) sql += @"  and t.semestre = @semestre";
            if (dreId > 0) sql += @" and dre.id = @dreId";
            if (ueId > 0) sql += @"  and ue.id = @ueId";
            sql += $@"    group by t.modalidade_codigo,{agrupamentoTurma} t.ano 
                         order by t.modalidade_codigo,{agrupamentoTurma} t.ano";

            return await database
                .Conexao
                .QueryAsync<FrequenciaGlobalPorAnoDto>(sql, new { modalidade, dreId, ueId, anoLetivo, tipoConsolidacao = TipoConsolidadoFrequencia.Anual });
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

        public async Task<IEnumerable<FrequenciaGlobalMensalSemanalDto>> ObterFrequenciasConsolidadasPorTurmaMensalSemestral(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, DateTime dataInicio, DateTime datafim, int tipoConsolidadoFrequencia, int semestre, bool visaoDre = false)
        {
            var selectSQL = string.Empty;

            if (visaoDre)
                selectSQL = "select dre.abreviacao as AbreviacaoDre, ";
            else if (ueId == -99)
                selectSQL = "select t.modalidade_codigo as ModalidadeTurma, t.ano AnoTurma, ";
            else if (ueId != -99 && !visaoDre)
                selectSQL = "select t.modalidade_codigo as ModalidadeTurma, t.nome as NomeTurma, ";

            selectSQL += @$"       dre.dre_id DreCodigo,
                                   cft.quantidade_acima_minimo_frequencia QuantidadeAcimaMinimoFrequencia, 
                                   cft.quantidade_abaixo_minimo_frequencia QuantidadeAbaixoMinimoFrequencia,
                                   cft.total_aulas TotalAulas, 
                                   cft.total_frequencias TotalFrequencias     
                            from consolidacao_frequencia_turma cft
                                join turma t on cft.turma_id = t.id
                                join ue on t.ue_id = ue.id
                                join dre on dre.id = ue.dre_id
                            where t.ano_letivo = @anoLetivo
                              and t.modalidade_codigo = @modalidade
                              and cft.tipo_consolidacao = @tipoConsolidadoFrequencia ";

            if (anoTurma != "-99")
                selectSQL += "and t.ano = @anoTurma ";

            if (dreId != -99)
                selectSQL += "and dre.id = @dreId ";

            if (ueId != -99)
                selectSQL += "and ue.id = @ueId ";

            if (semestre > 0)
                selectSQL += "and t.semestre = @semestre ";

            selectSQL += "and cft.periodo_inicio = @dataInicio and cft.periodo_fim = @datafim";

            var frequencias = await database.Conexao.QueryAsync<FrequenciaGlobalMensalSemanalDto>(selectSQL, new
            {
                dreId,
                ueId,
                anoLetivo,
                modalidade,
                dataInicio,
                datafim,
                anoTurma,
                tipoConsolidadoFrequencia,
                semestre
            });

            return frequencias.OrderBy(f => f.Descricao).ThenBy(f => f.DreCodigo).ToList();
        }

        public async Task<IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>> ObterQuantitativoAlunosFrequenciaBaixaPorTurma(int anoLetivo, TipoTurma tipoTurma)
        {
            var query = @" select SUM(cfam.quantidade_abaixo_minimo_frequencia) as quantidadeAbaixoMinimoFrequencia, 
                                  t.turma_id as codigoTurma
                           from consolidacao_frequencia_turma cfam
                                inner join turma t on t.id = cfam.turma_id 
                          where t.ano_letivo = @anoLetivo
                            and t.tipo_turma = @tipoTurma
                          group by t.turma_id";

            return await database.Conexao.QueryAsync<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>(query, new { anoLetivo, tipoTurma = (int)tipoTurma });
        }
    }
}