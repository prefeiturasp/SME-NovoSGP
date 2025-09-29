using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPapConsulta : IRepositorioPapConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioPapConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }
        public async Task<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>> ObterContagemDificuldadesConsolidadaGeral(int anoLetivo)
        {
            var query = @$"
            WITH RECURSIVE componentes_filtro AS (
                SELECT
                    {(int)TipoPap.PapColaborativo} as tipoPap,
                    array[{PainelEducacionalConstants.COD_COMPONENTES_CURRICULARES_PARA_INDICADORES_PAP
                            .FirstOrDefault(d => d.Key == TipoPap.PapColaborativo).Value}] as disciplinas
                UNION ALL
                SELECT
                    {(int)TipoPap.RecuperacaoAprendizagens} as tipoPap,
                    array[{PainelEducacionalConstants.COD_COMPONENTES_CURRICULARES_PARA_INDICADORES_PAP
                            .FirstOrDefault(d => d.Key == TipoPap.RecuperacaoAprendizagens).Value}] as disciplinas
                UNION ALL
                SELECT
                    {(int)TipoPap.Pap2Ano} as tipoPap,
                    array[{PainelEducacionalConstants.COD_COMPONENTES_CURRICULARES_PARA_INDICADORES_PAP
                            .FirstOrDefault(d => d.Key == TipoPap.Pap2Ano).Value}] as disciplinas
            ),
            turmas_relevantes AS (
                SELECT DISTINCT
                    t.id as turma_id,
                    t.ano_letivo,
                    cf.tipoPap,
                    dre.dre_id as codigoDre,
                    ue.ue_id as codigoUe
                FROM turma t
                INNER JOIN ue ON ue.id = t.ue_id
                INNER JOIN dre ON dre.id = ue.dre_id
                CROSS JOIN componentes_filtro cf
                WHERE t.modalidade_codigo = {(int)Modalidade.Fundamental}
                  AND t.ano_letivo = @anoLetivo
                  AND EXISTS (
                      SELECT 1
                      FROM aula a
                      WHERE a.turma_id = t.turma_id
                        AND NOT a.excluido
                        AND a.disciplina_id IS NOT NULL
                        AND a.disciplina_id::text ~ '^[0-9]+$'
                        AND a.disciplina_id::bigint = ANY(cf.disciplinas)
                  )
            ),
            ultimo_relatorio AS (
                SELECT
                    rppt.id as ultimo_relatorio_id,
                    tr.tipoPap,
                    tr.ano_letivo,
                    tr.codigoDre,
                    tr.codigoUe,
                    row_number() OVER (PARTITION BY rppt.turma_id ORDER BY rppt.id DESC) as rn
                FROM relatorio_periodico_pap_turma rppt
                INNER JOIN turmas_relevantes tr ON tr.turma_id = rppt.turma_id
                WHERE NOT rppt.excluido
            ),
            respostas_dificuldades AS MATERIALIZED (
                SELECT
                    rppr.resposta_id,
                    or_opcao.nome as nome_dificuldade,
                    ur.tipoPap,
                    ur.ano_letivo,
                    ur.codigoDre,
                    ur.codigoUe
                FROM ultimo_relatorio ur
                INNER JOIN relatorio_periodico_pap_aluno rppa ON rppa.relatorio_periodico_pap_turma_id = ur.ultimo_relatorio_id
                INNER JOIN relatorio_periodico_pap_secao rpps ON rpps.relatorio_periodico_pap_aluno_id = rppa.id
                INNER JOIN relatorio_periodico_pap_questao rppq ON rppq.relatorio_periodico_pap_secao_id = rpps.id
                INNER JOIN questao q ON q.id = rppq.questao_id
                INNER JOIN relatorio_periodico_pap_resposta rppr ON rppr.relatorio_periodico_pap_questao_id = rppq.id
                INNER JOIN opcao_resposta or_opcao ON or_opcao.id = rppr.resposta_id
                WHERE ur.rn = 1 AND NOT rppa.excluido AND NOT rpps.excluido AND NOT rppq.excluido AND NOT rppr.excluido AND NOT q.excluido AND NOT or_opcao.excluido
                  AND rppr.resposta_id IS NOT NULL
                  AND q.nome_componente = 'DIFIC_APRESENTADAS'
            ),
            uniao_visoes AS (
                -- Visão UE
                SELECT
                    'UE' as abrangencia,
                    rd.tipoPap,
                    rd.codigoDre,
                    rd.codigoUe,
                    rd.ano_letivo,
                    CASE WHEN td.resposta_id IS NOT NULL THEN rd.resposta_id ELSE 0 END AS respostaId,
                    CASE WHEN td.resposta_id IS NOT NULL THEN rd.nome_dificuldade ELSE 'Outros' END AS nomeDificuldade
                FROM respostas_dificuldades rd
                LEFT JOIN (
                    SELECT resposta_id, codigoUe, ano_letivo FROM (
                        SELECT resposta_id, codigoUe, ano_letivo, RANK() OVER (PARTITION BY codigoUe, ano_letivo ORDER BY COUNT(*) DESC) as ranking
                        FROM respostas_dificuldades
                        GROUP BY resposta_id, codigoUe, ano_letivo
                    ) classificados
                    WHERE ranking <= {PainelEducacionalConstants.QTD_INDICADORES_PAP}
                ) td ON rd.resposta_id = td.resposta_id AND rd.codigoUe = td.codigoUe AND rd.ano_letivo = td.ano_letivo

                UNION ALL

                -- Visão DRE
                SELECT
                    'DRE' as abrangencia,
                    rd.tipoPap,
                    rd.codigoDre,
                    NULL as codigoUe,
                    rd.ano_letivo,
                    CASE WHEN td.resposta_id IS NOT NULL THEN rd.resposta_id ELSE 0 END AS respostaId,
                    CASE WHEN td.resposta_id IS NOT NULL THEN rd.nome_dificuldade ELSE 'Outros' END AS nomeDificuldade
                FROM respostas_dificuldades rd
                LEFT JOIN (
                    SELECT resposta_id, codigoDre, ano_letivo FROM (
                        SELECT resposta_id, codigoDre, ano_letivo, RANK() OVER (PARTITION BY codigoDre, ano_letivo ORDER BY COUNT(*) DESC) as ranking
                        FROM respostas_dificuldades
                        GROUP BY resposta_id, codigoDre, ano_letivo
                    ) classificados
                    WHERE ranking <= {PainelEducacionalConstants.QTD_INDICADORES_PAP}
                ) td ON rd.resposta_id = td.resposta_id AND rd.codigoDre = td.codigoDre AND rd.ano_letivo = td.ano_letivo

                UNION ALL

                -- Visão SME
                SELECT
                    'SME' as abrangencia,
                    rd.tipoPap,
                    NULL as codigoDre,
                    NULL as codigoUe,
                    rd.ano_letivo,
                    CASE WHEN td.resposta_id IS NOT NULL THEN rd.resposta_id ELSE 0 END AS respostaId,
                    CASE WHEN td.resposta_id IS NOT NULL THEN rd.nome_dificuldade ELSE 'Outros' END AS nomeDificuldade
                FROM respostas_dificuldades rd
                LEFT JOIN (
                    SELECT resposta_id, ano_letivo FROM (
                        SELECT resposta_id, ano_letivo, RANK() OVER (PARTITION BY ano_letivo ORDER BY COUNT(*) DESC) as ranking
                        FROM respostas_dificuldades
                        GROUP BY resposta_id, ano_letivo
                    ) classificados
                    WHERE ranking <= {PainelEducacionalConstants.QTD_INDICADORES_PAP}
                ) td ON rd.resposta_id = td.resposta_id AND rd.ano_letivo = td.ano_letivo
            )
            SELECT
                abrangencia,
                tipoPap,
                codigoDre,
                codigoUe,
                ano_letivo,
                respostaId,
                nomeDificuldade,
                COUNT(*) as quantidade
            FROM uniao_visoes
            GROUP BY
                abrangencia,
                tipoPap,
                codigoDre,
                codigoUe,
                ano_letivo,
                respostaId,
                nomeDificuldade;";

            return await database.Conexao.QueryAsync<ContagemDificuldadeIndicadoresPapPorTipoDto>(query, new { anoLetivo });
        }
    }
}