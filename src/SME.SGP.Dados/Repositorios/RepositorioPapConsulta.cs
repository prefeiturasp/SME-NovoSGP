using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPapConsulta : IRepositorioPapConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioPapConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }
        public async Task<IEnumerable<ContagemDificuldadePorTipoDto>> ObterContagemDificuldadesPorTipoDetalhado(TipoPap tipoPap, string codigoDre = null, string codigoUe = null)
        {
            var query = @"
                WITH componentes_filtro AS (
                    SELECT CASE 
                        WHEN @tipoPap = 1 THEN array[1770] -- PapColaborativo
                        WHEN @tipoPap = 2 THEN array[1322] -- RecuperacaoAprendizagens  
                        WHEN @tipoPap = 3 THEN array[1804, 1805] -- Pap2Ano
                        ELSE array[]::integer[]
                    END as disciplinas
                ),
                anos_filtro AS (
                    SELECT 
                        extract(year from now())::integer as ano_atual,
                        (extract(year from now()) - 1)::integer as ano_anterior
                ),
                turmas_relevantes AS (
                    SELECT DISTINCT t.id as turma_id, t.ano_letivo, dre.dre_id as codigo_dre, dre.nome as nome_dre, ue.ue_id as codigo_ue, ue.nome as nome_ue
                    FROM turma t
                    INNER JOIN ue ON ue.id = t.ue_id
                    INNER JOIN dre ON dre.id = ue.dre_id
                    WHERE t.modalidade_codigo = 5
                      AND t.ano_letivo >= (SELECT ano_anterior FROM anos_filtro)
                      AND t.ano_letivo <= (SELECT ano_atual FROM anos_filtro)
                      AND (@codigoDre IS NULL OR dre.dre_id = @codigoDre)
                      AND (@codigoUe IS NULL OR ue.ue_id = @codigoUe)
                      AND EXISTS (
                          SELECT 1 
                          FROM aula a, componentes_filtro cf
                          WHERE a.turma_id = t.turma_id
                            AND NOT a.excluido
                            AND a.disciplina_id IS NOT NULL
                            AND a.disciplina_id != ''
                            AND a.disciplina_id ~ '^[0-9]+$'
                            AND a.disciplina_id::integer = ANY(cf.disciplinas)
                      )
                ),
                ultimo_relatorio AS (
                    SELECT 
                        rppt.turma_id,
                        rppt.id as ultimo_relatorio_id,
                        tr.ano_letivo,
                        tr.codigo_dre,
                        tr.nome_dre,
                        tr.codigo_ue,
                        tr.nome_ue,
                        row_number() OVER (PARTITION BY rppt.turma_id ORDER BY rppt.id DESC) as rn
                    FROM relatorio_periodico_pap_turma rppt
                    INNER JOIN turmas_relevantes tr ON tr.turma_id = rppt.turma_id
                    WHERE NOT rppt.excluido
                ),
                respostas_dificuldades AS (
                    SELECT 
                        rppr.resposta_id,
                        or_opcao.nome as nome_dificuldade,
                        ur.ano_letivo,
                        ur.codigo_dre,
                        ur.nome_dre,
                        ur.codigo_ue,
                        ur.nome_ue
                    FROM ultimo_relatorio ur
                    INNER JOIN relatorio_periodico_pap_aluno rppa ON rppa.relatorio_periodico_pap_turma_id = ur.ultimo_relatorio_id
                    INNER JOIN relatorio_periodico_pap_secao rpps ON rpps.relatorio_periodico_pap_aluno_id = rppa.id
                    INNER JOIN secao_relatorio_periodico_pap srpp ON srpp.id = rpps.secao_relatorio_periodico_pap_id
                    INNER JOIN relatorio_periodico_pap_questao rppq ON rppq.relatorio_periodico_pap_secao_id = rpps.id
                    INNER JOIN questao q ON q.id = rppq.questao_id
                    INNER JOIN relatorio_periodico_pap_resposta rppr ON rppr.relatorio_periodico_pap_questao_id = rppq.id
                    INNER JOIN opcao_resposta or_opcao ON or_opcao.id = rppr.resposta_id
                    WHERE ur.rn = 1
                      AND NOT rppa.excluido
                      AND NOT rpps.excluido
                      AND NOT rppq.excluido
                      AND NOT rppr.excluido
                      AND NOT q.excluido
                      AND NOT or_opcao.excluido
                      AND rppr.resposta_id IS NOT NULL
                      AND (
                          (srpp.nome_componente = 'SECAO_DIFIC_APRES' AND q.nome_componente = 'DIFIC_APRESENTADAS') OR
                          q.nome_componente = 'DIFIC_APRESENTADAS'
                      )
                ),
                contagem_respostas_por_ue AS (
                    SELECT 
                        ano_letivo,
                        resposta_id,
                        nome_dificuldade,
                        codigo_dre,
                        nome_dre,
                        codigo_ue,
                        nome_ue,
                        count(*) as quantidade
                    FROM respostas_dificuldades
                    GROUP BY ano_letivo, resposta_id, nome_dificuldade, codigo_dre, nome_dre, codigo_ue, nome_ue
                ),
                ranking_por_ue AS (
                    SELECT 
                        ano_letivo,
                        resposta_id,
                        nome_dificuldade,
                        codigo_dre,
                        nome_dre,
                        codigo_ue,
                        nome_ue,
                        quantidade,
                        row_number() OVER (
                            PARTITION BY codigo_ue, ano_letivo 
                            ORDER BY quantidade DESC, resposta_id
                        ) as posicao
                    FROM contagem_respostas_por_ue
                ),
                top_dificuldades_por_ue AS (
                    SELECT 
                        codigo_dre,
                        nome_dre,
                        codigo_ue,
                        nome_ue,
                        sum(CASE WHEN posicao = 1 THEN quantidade ELSE 0 END) as DificuldadeAprendizagemTop1,
                        sum(CASE WHEN posicao = 2 THEN quantidade ELSE 0 END) as DificuldadeAprendizagemTop2,
                        sum(CASE WHEN posicao > 2 THEN quantidade ELSE 0 END) as OutrasDificuldadesAprendizagem,
                        max(CASE WHEN posicao = 1 THEN nome_dificuldade END) as NomeDificuldadeTop1,
                        max(CASE WHEN posicao = 2 THEN nome_dificuldade END) as NomeDificuldadeTop2
                    FROM ranking_por_ue
                    GROUP BY codigo_dre, nome_dre, codigo_ue, nome_ue
                )
                SELECT 
                    @tipoPap as TipoPap,
                    codigo_dre as CodigoDre,
                    nome_dre as NomeDre,
                    codigo_ue as CodigoUe,
                    nome_ue as NomeUe,
                    DificuldadeAprendizagemTop1 as QuantidadeEstudantesDificuldadeTop1,
                    DificuldadeAprendizagemTop2 as QuantidadeEstudantesDificuldadeTop2,
                    OutrasDificuldadesAprendizagem,
                    coalesce(NomeDificuldadeTop1, '') as NomeDificuldadeTop1,
                    coalesce(NomeDificuldadeTop2, '') as NomeDificuldadeTop2
                FROM top_dificuldades_por_ue
                WHERE (DificuldadeAprendizagemTop1 > 0 OR DificuldadeAprendizagemTop2 > 0 OR OutrasDificuldadesAprendizagem > 0)
                ORDER BY codigo_dre, codigo_ue;";

            var resultado = await database.Conexao.QueryAsync<ContagemDificuldadePorTipoDto>(
                query,
                new { 
                    tipoPap = (int)tipoPap,
                    codigoDre = string.IsNullOrWhiteSpace(codigoDre) ? null : codigoDre,
                    codigoUe = string.IsNullOrWhiteSpace(codigoUe) ? null : codigoUe
                });

            return resultado?.Select(r => 
            {
                r.TipoPap = tipoPap;
                return r;
            }) ?? new List<ContagemDificuldadePorTipoDto>();
        }

        public async Task<IEnumerable<ContagemDificuldadePorTipoDto>> ObterContagemDificuldadesConsolidadaGeral()
        {
            var query = @"
                WITH componentes_filtro AS (
                    SELECT 
                        1 as tipo_pap, array[1770] as disciplinas UNION ALL
                    SELECT 
                        2 as tipo_pap, array[1322] as disciplinas UNION ALL  
                    SELECT 
                        3 as tipo_pap, array[1804, 1805] as disciplinas
                ),
                anos_filtro AS (
                    SELECT 
                        extract(year from now())::integer as ano_atual,
                        (extract(year from now()) - 1)::integer as ano_anterior
                ),
                turmas_relevantes AS (
                    SELECT DISTINCT 
                        t.id as turma_id, 
                        t.ano_letivo, 
                        cf.tipo_pap,
                        dre.dre_id as codigo_dre, 
                        dre.nome as nome_dre, 
                        ue.ue_id as codigo_ue, 
                        ue.nome as nome_ue
                    FROM turma t
                    INNER JOIN ue ON ue.id = t.ue_id
                    INNER JOIN dre ON dre.id = ue.dre_id
                    CROSS JOIN componentes_filtro cf
                    WHERE t.modalidade_codigo = 5
                      AND t.ano_letivo >= (SELECT ano_anterior FROM anos_filtro)
                      AND t.ano_letivo <= (SELECT ano_atual FROM anos_filtro)
                      AND EXISTS (
                          SELECT 1 
                          FROM aula a
                          WHERE a.turma_id = t.turma_id
                            AND NOT a.excluido
                            AND a.disciplina_id IS NOT NULL
                            AND a.disciplina_id != ''
                            AND a.disciplina_id ~ '^[0-9]+$'
                            AND a.disciplina_id::integer = ANY(cf.disciplinas)
                      )
                ),
                ultimo_relatorio AS (
                    SELECT 
                        rppt.turma_id,
                        rppt.id as ultimo_relatorio_id,
                        tr.tipo_pap,
                        tr.ano_letivo,
                        tr.codigo_dre,
                        tr.nome_dre,
                        tr.codigo_ue,
                        tr.nome_ue,
                        row_number() OVER (PARTITION BY rppt.turma_id ORDER BY rppt.id DESC) as rn
                    FROM relatorio_periodico_pap_turma rppt
                    INNER JOIN turmas_relevantes tr ON tr.turma_id = rppt.turma_id
                    WHERE NOT rppt.excluido
                ),
                respostas_dificuldades AS (
                    SELECT 
                        rppr.resposta_id,
                        or_opcao.nome as nome_dificuldade,
                        ur.tipo_pap,
                        ur.ano_letivo
                    FROM ultimo_relatorio ur
                    INNER JOIN relatorio_periodico_pap_aluno rppa ON rppa.relatorio_periodico_pap_turma_id = ur.ultimo_relatorio_id
                    INNER JOIN relatorio_periodico_pap_secao rpps ON rpps.relatorio_periodico_pap_aluno_id = rppa.id
                    INNER JOIN secao_relatorio_periodico_pap srpp ON srpp.id = rpps.secao_relatorio_periodico_pap_id
                    INNER JOIN relatorio_periodico_pap_questao rppq ON rppq.relatorio_periodico_pap_secao_id = rpps.id
                    INNER JOIN questao q ON q.id = rppq.questao_id
                    INNER JOIN relatorio_periodico_pap_resposta rppr ON rppr.relatorio_periodico_pap_questao_id = rppq.id
                    INNER JOIN opcao_resposta or_opcao ON or_opcao.id = rppr.resposta_id
                    WHERE ur.rn = 1
                      AND NOT rppa.excluido
                      AND NOT rpps.excluido
                      AND NOT rppq.excluido
                      AND NOT rppr.excluido
                      AND NOT q.excluido
                      AND NOT or_opcao.excluido
                      AND rppr.resposta_id IS NOT NULL
                      AND (
                          (srpp.nome_componente = 'SECAO_DIFIC_APRES' AND q.nome_componente = 'DIFIC_APRESENTADAS') OR
                          q.nome_componente = 'DIFIC_APRESENTADAS'
                      )
                ),
                contagem_respostas_geral AS (
                    SELECT 
                        tipo_pap,
                        resposta_id,
                        nome_dificuldade,
                        count(*) as quantidade
                    FROM respostas_dificuldades
                    GROUP BY tipo_pap, resposta_id, nome_dificuldade
                ),
                ranking_geral AS (
                    SELECT 
                        tipo_pap,
                        resposta_id,
                        nome_dificuldade,
                        quantidade,
                        row_number() OVER (
                            PARTITION BY tipo_pap
                            ORDER BY quantidade DESC, resposta_id
                        ) as posicao
                    FROM contagem_respostas_geral
                ),
                top_dificuldades_geral AS (
                    SELECT 
                        tipo_pap,
                        sum(CASE WHEN posicao = 1 THEN quantidade ELSE 0 END) as DificuldadeAprendizagemTop1,
                        sum(CASE WHEN posicao = 2 THEN quantidade ELSE 0 END) as DificuldadeAprendizagemTop2,
                        sum(CASE WHEN posicao > 2 THEN quantidade ELSE 0 END) as OutrasDificuldadesAprendizagem,
                        max(CASE WHEN posicao = 1 THEN nome_dificuldade END) as NomeDificuldadeTop1,
                        max(CASE WHEN posicao = 2 THEN nome_dificuldade END) as NomeDificuldadeTop2
                    FROM ranking_geral
                    GROUP BY tipo_pap
                )
                SELECT 
                    tipo_pap as TipoPap,
                    '' as CodigoDre,
                    '' as NomeDre,
                    '' as CodigoUe,
                    '' as NomeUe,
                    DificuldadeAprendizagemTop1 as QuantidadeEstudantesDificuldadeTop1,
                    DificuldadeAprendizagemTop2 as QuantidadeEstudantesDificuldadeTop2,
                    OutrasDificuldadesAprendizagem,
                    coalesce(NomeDificuldadeTop1, '') as NomeDificuldadeTop1,
                    coalesce(NomeDificuldadeTop2, '') as NomeDificuldadeTop2
                FROM top_dificuldades_geral
                WHERE (DificuldadeAprendizagemTop1 > 0 OR DificuldadeAprendizagemTop2 > 0 OR OutrasDificuldadesAprendizagem > 0)
                ORDER BY tipo_pap;";

            var resultado = await database.Conexao.QueryAsync<ContagemDificuldadePorTipoDto>(query);

            return resultado?.Select(r => 
            {
                r.TipoPap = (TipoPap)r.TipoPap;
                return r;
            }) ?? new List<ContagemDificuldadePorTipoDto>();
        }
    }
}