using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interfaces;
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

        public async Task<ContagemDificuldadePorTipoDto> ObterContagemDificuldadesPorTipo(TipoPap tipoPap)
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
                    SELECT DISTINCT t.id as turma_id, t.ano_letivo
                    FROM turma t
                    WHERE t.modalidade_codigo = 5
                      AND t.ano_letivo >= (SELECT ano_anterior FROM anos_filtro)
                      AND t.ano_letivo <= (SELECT ano_atual FROM anos_filtro)
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
                relatorios_validos AS (
                    SELECT 
                        rppt.turma_id,
                        rppt.id,
                        tr.ano_letivo,
                        row_number() OVER (PARTITION BY rppt.turma_id ORDER BY rppt.id DESC) as rn
                    FROM relatorio_periodico_pap_turma rppt
                    INNER JOIN turmas_relevantes tr ON tr.turma_id = rppt.turma_id
                    WHERE NOT rppt.excluido
                ),
                ultimo_relatorio AS (
                    SELECT turma_id, id as ultimo_relatorio_id, ano_letivo
                    FROM relatorios_validos
                    WHERE rn = 1
                ),
                respostas_dificuldades AS (
                    SELECT 
                        rppr.resposta_id,
                        ur.ano_letivo
                    FROM ultimo_relatorio ur
                    INNER JOIN relatorio_periodico_pap_aluno rppa ON rppa.relatorio_periodico_pap_turma_id = ur.ultimo_relatorio_id
                    INNER JOIN relatorio_periodico_pap_secao rpps ON rpps.relatorio_periodico_pap_aluno_id = rppa.id
                    INNER JOIN secao_relatorio_periodico_pap srpp ON srpp.id = rpps.secao_relatorio_periodico_pap_id
                    INNER JOIN relatorio_periodico_pap_questao rppq ON rppq.relatorio_periodico_pap_secao_id = rpps.id
                    INNER JOIN questao q ON q.id = rppq.questao_id
                    INNER JOIN relatorio_periodico_pap_resposta rppr ON rppr.relatorio_periodico_pap_questao_id = rppq.id
                    WHERE NOT rppa.excluido
                      AND NOT rpps.excluido
                      AND NOT rppq.excluido
                      AND NOT rppr.excluido
                      AND NOT q.excluido
                      AND rppr.resposta_id IS NOT NULL
                      AND (
                          (srpp.nome_componente = 'SECAO_DIFIC_APRES' AND q.nome_componente = 'DIFIC_APRESENTADAS') OR
                          q.nome_componente = 'DIFIC_APRESENTADAS'
                      )
                ),
                contagem_respostas AS (
                    SELECT 
                        ano_letivo,
                        resposta_id,
                        count(*) as quantidade
                    FROM respostas_dificuldades
                    GROUP BY ano_letivo, resposta_id
                ),
                ranking_dificuldades AS (
                    SELECT 
                        ano_letivo,
                        resposta_id,
                        quantidade,
                        rank() OVER (PARTITION BY ano_letivo ORDER BY quantidade DESC) as posicao
                    FROM contagem_respostas
                ),
                agrupamento_final AS (
                    SELECT 
                        ano_letivo,
                        CASE 
                            WHEN posicao = 1 THEN 'DificuldadeAprendizagem1'
                            WHEN posicao = 2 THEN 'DificuldadeAprendizagem2'
                            ELSE 'OutrasDificuldadesAprendizagem'
                        END as tipo_dificuldade,
                        sum(quantidade) as total_quantidade
                    FROM ranking_dificuldades
                    GROUP BY ano_letivo, 
                        CASE 
                            WHEN posicao = 1 THEN 'DificuldadeAprendizagem1'
                            WHEN posicao = 2 THEN 'DificuldadeAprendizagem2'
                            ELSE 'OutrasDificuldadesAprendizagem'
                        END
                ),
                dados_priorizados AS (
                    SELECT 
                        tipo_dificuldade,
                        total_quantidade,
                        row_number() OVER (
                            PARTITION BY tipo_dificuldade 
                            ORDER BY 
                                CASE WHEN ano_letivo = (SELECT ano_atual FROM anos_filtro) THEN 1 ELSE 2 END,
                                ano_letivo DESC
                        ) as prioridade
                    FROM agrupamento_final
                )
                SELECT 
                    @tipoPap as TipoPap,
                    coalesce(max(CASE WHEN tipo_dificuldade = 'DificuldadeAprendizagem1' THEN total_quantidade END), 0) as DificuldadeAprendizagem1,
                    coalesce(max(CASE WHEN tipo_dificuldade = 'DificuldadeAprendizagem2' THEN total_quantidade END), 0) as DificuldadeAprendizagem2,
                    coalesce(max(CASE WHEN tipo_dificuldade = 'OutrasDificuldadesAprendizagem' THEN total_quantidade END), 0) as OutrasDificuldadesAprendizagem
                FROM dados_priorizados
                WHERE prioridade = 1;";

            var resultado = await database.Conexao.QueryFirstOrDefaultAsync<ContagemDificuldadePorTipoDto>(
                query,
                new { tipoPap = (int)tipoPap });

            if (resultado == null)
            {
                resultado = new ContagemDificuldadePorTipoDto
                {
                    TipoPap = tipoPap,
                    DificuldadeAprendizagem1 = 0,
                    DificuldadeAprendizagem2 = 0,
                    OutrasDificuldadesAprendizagem = 0
                };
            }
            else
            {
                resultado.TipoPap = tipoPap; 
            }

            return resultado;
        }
    }
}