using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConselhoClasseConsolidado : RepositorioBase<ConselhoClasseConsolidadoTurmaAluno>, IRepositorioConselhoClasseConsolidado
    {
        public RepositorioConselhoClasseConsolidado(ISgpContext database) : base(database)
        {
        }


        public async Task<IEnumerable<ConselhoClasseConsolidadoTurmaAluno>> ObterConselhosClasseConsolidadoPorTurmaBimestreAsync(long turmaId, int situacaoConselhoClasse)
        {
            var query = new StringBuilder(@" select id, dt_atualizacao, status, aluno_codigo, parecer_conclusivo_id, turma_id,  
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido
                            from consolidado_conselho_classe_aluno_turma 
                          where not excluido 
                            and turma_id = @turmaId ");

            if (situacaoConselhoClasse != -99)
                query.AppendLine(@"and EXISTS(select 1 from consolidado_conselho_classe_aluno_turma
                                              where not excluido and turma_id = @turmaId 
                                                and status = @situacaoConselhoClasse)");

            return await database.Conexao.QueryAsync<ConselhoClasseConsolidadoTurmaAluno>(query.ToString(), new { turmaId, situacaoConselhoClasse });
        }
        public Task<ConselhoClasseConsolidadoTurmaAluno> ObterConselhoClasseConsolidadoPorTurmaBimestreAlunoAsync(long turmaId, string alunoCodigo)
        {
            var query = $@" select id, dt_atualizacao, status, aluno_codigo, parecer_conclusivo_id, turma_id,   
                                   criado_em, criado_por, alterado_em, alterado_por, criado_rf, alterado_rf, excluido           
                            from consolidado_conselho_classe_aluno_turma 
                            where not excluido 
                            and turma_id = @turmaId
                            and aluno_codigo = @alunoCodigo";

            return database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasseConsolidadoTurmaAluno>(query, new { turmaId, alunoCodigo });
        }

        public Task<IEnumerable<ConsolidacaoConselhoClasseAlunoMigracaoDto>> ObterFechamentoNotaAlunoOuConselhoClasseAsync(long turmaId)
        {
            var query = @"	                        
                            SELECT DISTINCT *
                            FROM   (SELECT fa.aluno_codigo AlunoCodigo,
                                           ft.turma_id TurmaId,
			                               fn.disciplina_id DisciplinaId,
                                           coalesce(ccn.nota,fn.nota) Nota,
                                           coalesce(ccn.conceito_id,fn.conceito_id) as ConceitoId,               
                                           pe.bimestre,
                                           cca.conselho_classe_parecer_id ParecerConclusivoId,
                                           coalesce(ccn.criado_em,fn.criado_em,cca.criado_em) criadoEm, 
                                           coalesce(ccn.criado_por,fn.criado_por,cca.criado_por) CriadoPor,  
                                           coalesce(ccn.criado_rf,fn.criado_rf,cca.criado_rf) CriadoRf,  
                                           cca.id ConselhoClasseAlunoId
                                    FROM   fechamento_turma ft
                                           LEFT JOIN periodo_escolar pe
                                                  ON pe.id = ft.periodo_escolar_id
                                           INNER JOIN turma t
                                                   ON t.id = ft.turma_id
                                           INNER JOIN ue
                                                   ON ue.id = t.ue_id
                                           INNER JOIN fechamento_turma_disciplina ftd
                                                   ON ftd.fechamento_turma_id = ft.id
                                           INNER JOIN fechamento_aluno fa
                                                   ON fa.fechamento_turma_disciplina_id = ftd.id
                                           INNER JOIN fechamento_nota fn
                                                   ON fn.fechamento_aluno_id = fa.id
                                           INNER JOIN componente_curricular comp
                                                   ON comp.id = fn.disciplina_id
                                           INNER JOIN conselho_classe cc
                                                   ON cc.fechamento_turma_id = ft.id
                                           INNER JOIN conselho_classe_aluno cca
                                                   ON cca.conselho_classe_id = cc.id
                                                      AND cca.aluno_codigo = fa.aluno_codigo
                                           LEFT JOIN conselho_classe_nota ccn
                                                  ON ccn.conselho_classe_aluno_id = cca.id
                                                     AND ccn.componente_curricular_codigo = fn.disciplina_id
                                    WHERE  ft.turma_id = @turmaId 
                                    UNION ALL
                                    SELECT fa.aluno_codigo AlunoCodigo,
                                            ft.turma_id TurmaId,
		                                    fn.disciplina_id DisciplinaId,
                                            coalesce(ccn.nota,fn.nota) Nota,
                                            coalesce(ccn.conceito_id,fn.conceito_id) as ConceitoId,               
                                            pe.bimestre,
                                            cca.conselho_classe_parecer_id ParecerConclusivoId,
                                            coalesce(ccn.criado_em,fn.criado_em,cca.criado_em) criadoEm,               
                                            coalesce(ccn.criado_por,fn.criado_por,cca.criado_por) CriadoPor,  
                                            coalesce(ccn.criado_rf,fn.criado_rf,cca.criado_rf) CriadoRf,                 
                                            cca.id ConselhoClasseAlunoId
                                    FROM   fechamento_turma ft
                                           LEFT JOIN periodo_escolar pe
                                                  ON pe.id = ft.periodo_escolar_id
                                           INNER JOIN turma t
                                                   ON t.id = ft.turma_id
                                           INNER JOIN ue
                                                   ON ue.id = t.ue_id
                                           INNER JOIN conselho_classe cc
                                                   ON cc.fechamento_turma_id = ft.id
                                           INNER JOIN conselho_classe_aluno cca
                                                   ON cca.conselho_classe_id = cc.id
                                           INNER JOIN conselho_classe_nota ccn
                                                   ON ccn.conselho_classe_aluno_id = cca.id
                                           INNER JOIN componente_curricular comp
                                                   ON comp.id = ccn.componente_curricular_codigo
                                           LEFT JOIN fechamento_turma_disciplina ftd
                                                  ON ftd.fechamento_turma_id = ft.id
                                           LEFT JOIN fechamento_aluno fa
                                                  ON fa.fechamento_turma_disciplina_id = ftd.id
                                                     AND cca.aluno_codigo = fa.aluno_codigo
                                           LEFT JOIN fechamento_nota fn
                                                  ON fn.fechamento_aluno_id = fa.id
                                                     AND ccn.componente_curricular_codigo = fn.disciplina_id
                                    WHERE  ft.turma_id = @turmaId) x   
                                    WHERE x.DisciplinaId is not null
                                    ORDER BY AlunoCodigo asc ,bimestre asc ,criadoEm desc";

            return (database.Conexao.QueryAsync<ConsolidacaoConselhoClasseAlunoMigracaoDto>(query, new { turmaId }));
        }

        public Task<long> ObterConselhoClasseConsolidadoPorTurmaAlunoAsync(long turmaId, string alunoCodigo)
        {
            var query = @"select id as ConsolidacaoId, 
                        turma_id as TurmaId,
                        aluno_codigo as AlunoCodigo
                        from consolidado_conselho_classe_aluno_turma 
                        where not excluido 
                        and turma_id = @turmaId 
                        and aluno_codigo = @alunoCodigo";

            return database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { turmaId, alunoCodigo });
        }
    }
}
