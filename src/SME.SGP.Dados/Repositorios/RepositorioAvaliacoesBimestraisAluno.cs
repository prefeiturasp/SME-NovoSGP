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
                        cc.codigo AS componente,
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
                        cc.codigo AS componente,
                        CASE 
                            WHEN fa.total_aulas > 0 THEN 
                                ROUND((fa.total_aulas - fa.total_ausencias + fa.total_compensacoes) * 100.0 / fa.total_aulas, 2)
                            ELSE 100.0
                        END AS percentual_frequencia
                    FROM fechamento_aluno fa
                    INNER JOIN fechamento_turma_disciplina ftd ON ftd.id = fa.fechamento_turma_disciplina_id
                    INNER JOIN consolidado_fechamento_componente_turma cfct ON cfct.turma_id = ftd.turma_id 
                        AND cfct.componente_curricular_id = ftd.disciplina_id
                        AND cfct.bimestre = @bimestre
                    INNER JOIN turma t ON t.id = cfct.turma_id
                    INNER JOIN componente_curricular cc ON cc.id = cfct.componente_curricular_id
                    WHERE fa.aluno_codigo = @codigoAluno
                    AND t.turma_id = @codigoTurma
                    AND t.ano_letivo = @anoLetivo
                    AND NOT fa.excluido
                    AND NOT ftd.excluido
                    AND NOT cfct.excluido
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
                        cc.codigo AS componente,
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
                        cc.codigo AS componente,
                        CASE 
                            WHEN fa.total_aulas > 0 THEN 
                                ROUND((fa.total_aulas - fa.total_ausencias + fa.total_compensacoes) * 100.0 / fa.total_aulas, 2)
                            ELSE 100.0
                        END AS percentual_frequencia
                    FROM fechamento_aluno fa
                    INNER JOIN fechamento_turma_disciplina ftd ON ftd.id = fa.fechamento_turma_disciplina_id
                    INNER JOIN consolidado_fechamento_componente_turma cfct ON cfct.turma_id = ftd.turma_id 
                        AND cfct.componente_curricular_id = ftd.disciplina_id
                        AND cfct.bimestre IS NULL
                    INNER JOIN turma t ON t.id = cfct.turma_id
                    INNER JOIN componente_curricular cc ON cc.id = cfct.componente_curricular_id
                    WHERE fa.aluno_codigo = @codigoAluno
                    AND t.turma_id = @codigoTurma
                    AND t.ano_letivo = @anoLetivo
                    AND NOT fa.excluido
                    AND NOT ftd.excluido
                    AND NOT cfct.excluido
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
