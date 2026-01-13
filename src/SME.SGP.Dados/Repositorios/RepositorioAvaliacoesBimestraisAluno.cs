using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAvaliacoesBimestraisAluno : IRepositorioAvaliacoesBimestraisAluno
    {
        private readonly ISgpContext database;

        public RepositorioAvaliacoesBimestraisAluno(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<IndicadorAvaliacaoDto>> ObterIndicadoresPorBimestre(string codigoAluno, long turmaId, string codigoTurma, int bimestre, int anoLetivo)
        {
            var query = @"
                WITH notas_bimestre AS (
                    SELECT 
                        cc.id AS componente,
                        COALESCE(ccn.conceito_id::text, ccn.nota::text) AS nota
                    FROM consolidado_conselho_classe_aluno_turma_nota ccn
                    INNER JOIN consolidado_conselho_classe_aluno_turma cca ON cca.id = ccn.consolidado_conselho_classe_aluno_turma_id
                    INNER JOIN turma t ON t.id = cca.turma_id
                    INNER JOIN componente_curricular cc ON cc.id = ccn.componente_curricular_id
                    WHERE cca.aluno_codigo = @codigoAluno
                    AND t.id = @turmaId
                    AND COALESCE(ccn.bimestre, 0) = @bimestre
                    AND t.ano_letivo = @anoLetivo
                    AND NOT cca.excluido
                ),
                frequencia_bimestre AS (
                    SELECT 
                        CASE 
                            WHEN fa.disciplina_id IS NOT NULL AND fa.disciplina_id != '' AND fa.disciplina_id ~ '^\d+$'
                            THEN CAST(fa.disciplina_id AS BIGINT) 
                            ELSE NULL
                        END AS componente,
                        CASE 
                            WHEN fa.total_aulas > 0 THEN 
                                ROUND((fa.total_aulas - fa.total_ausencias + fa.total_compensacoes) * 100.0 / fa.total_aulas, 2)
                            ELSE 100.0
                        END AS percentual_frequencia
                    FROM frequencia_aluno fa
                    INNER JOIN turma t ON t.turma_id = fa.turma_id
                    WHERE fa.codigo_aluno = @codigoAluno
                    AND t.id = @turmaId
                    AND fa.bimestre = @bimestre
                    AND t.ano_letivo = @anoLetivo
                    AND fa.disciplina_id IS NOT NULL 
                    AND fa.disciplina_id != '' 
                    AND fa.disciplina_id ~ '^\d+$'
                )
                SELECT 
                    COALESCE(nb.componente, fb.componente) AS Componente,
                    nb.nota AS Nota,
                    fb.percentual_frequencia AS PercentualFrequencia
                FROM notas_bimestre nb
                FULL OUTER JOIN frequencia_bimestre fb ON nb.componente = fb.componente
                WHERE (nb.componente IS NOT NULL OR fb.componente IS NOT NULL)
                ORDER BY Componente";

            return await database.Conexao.QueryAsync<IndicadorAvaliacaoDto>(query, new { codigoAluno, turmaId, codigoTurma, bimestre, anoLetivo });
        }

        public async Task<IEnumerable<IndicadorAvaliacaoDto>> ObterIndicadoresAvaliacaoFinal(string codigoAluno, long turmaId, string codigoTurma, int anoLetivo)
        {
            var query = @"
                WITH notas_final AS (
                    SELECT 
                        cc.id AS componente,
                        COALESCE(ccn.conceito_id::text, ccn.nota::text) AS nota
                    FROM consolidado_conselho_classe_aluno_turma_nota ccn
                    INNER JOIN consolidado_conselho_classe_aluno_turma cca ON cca.id = ccn.consolidado_conselho_classe_aluno_turma_id
                    INNER JOIN turma t ON t.id = cca.turma_id
                    INNER JOIN componente_curricular cc ON cc.id = ccn.componente_curricular_id
                    WHERE cca.aluno_codigo = @codigoAluno
                    AND t.id = @turmaId
                    AND ccn.bimestre IS NULL -- Fechamento final não possui bimestre
                    AND t.ano_letivo = @anoLetivo
                    AND NOT cca.excluido
                ),
                frequencia_final AS (
                    SELECT 
                        CASE 
                            WHEN fa.disciplina_id IS NOT NULL AND fa.disciplina_id != '' AND fa.disciplina_id ~ '^\d+$'
                            THEN CAST(fa.disciplina_id AS BIGINT) 
                            ELSE NULL
                        END AS componente,
                        CASE 
                            WHEN SUM(fa.total_aulas) > 0 THEN 
                                ROUND((SUM(fa.total_aulas) - SUM(fa.total_ausencias) + SUM(fa.total_compensacoes)) * 100.0 / SUM(fa.total_aulas), 2)
                            ELSE NULL
                        END AS percentual_frequencia
                    FROM frequencia_aluno fa
                    INNER JOIN turma t ON t.turma_id = fa.turma_id
                    WHERE fa.codigo_aluno = @codigoAluno
                    AND t.id = @turmaId
                    AND t.ano_letivo = @anoLetivo
                    AND fa.disciplina_id IS NOT NULL 
                    AND fa.disciplina_id != '' 
                    AND fa.disciplina_id ~ '^\d+$'
                    GROUP BY fa.disciplina_id
                    HAVING SUM(fa.total_aulas) > 0
                )
                SELECT 
                    COALESCE(nf.componente, ff.componente) AS Componente,
                    nf.nota AS Nota,
                    ff.percentual_frequencia AS PercentualFrequencia
                FROM notas_final nf
                FULL OUTER JOIN frequencia_final ff ON nf.componente = ff.componente
                WHERE (nf.componente IS NOT NULL OR ff.componente IS NOT NULL)
                ORDER BY Componente";

            return await database.Conexao.QueryAsync<IndicadorAvaliacaoDto>(query, new { codigoAluno, turmaId, codigoTurma, anoLetivo });
        }
    }
}