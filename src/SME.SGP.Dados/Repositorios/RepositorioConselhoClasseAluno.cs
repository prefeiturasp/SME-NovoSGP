using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAluno : RepositorioBase<ConselhoClasseAluno>, IRepositorioConselhoClasseAluno
    {
        public RepositorioConselhoClasseAluno(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo)
        {
            var query = $@"select distinct * from (SELECT 0                AS ConselhoClasseAlunoId,
                                   fn.disciplina_id AS ComponenteCurricularCodigo,
                                   fn.conceito_id   AS ConceitoId,
                                   fn.nota          AS Nota
                            FROM   fechamento_turma ft
                                   LEFT JOIN periodo_escolar pe
                                          ON pe.id = ft.periodo_escolar_id
                                   INNER JOIN turma t
                                           ON t.id = ft.turma_id
                                   INNER JOIN fechamento_turma_disciplina ftd
                                           ON ftd.fechamento_turma_id = ft.id
                                   INNER JOIN fechamento_aluno fa
                                           ON fa.fechamento_turma_disciplina_id = ftd.id
                                   INNER JOIN fechamento_nota fn
                                           ON fn.fechamento_aluno_id = fa.id 
                                   inner join componente_curricular ccr on fn.disciplina_id = ccr.id
                            WHERE  t.turma_id = ANY(@turmasCodigos)
                                   AND fa.aluno_codigo = @alunoCodigo
                                   AND pe.bimestre IS NULL
                                   and ccr.permite_lancamento_nota
                            UNION
                            SELECT cca.id                                    AS ConselhoClasseAlunoId,
                                   ccn.componente_curricular_codigo          AS ComponenteCurricularCodigo,
                                   ccn.conceito_id AS ConceitoId,
                                   ccn.nota               AS Nota
                            FROM   fechamento_turma ft
                                   LEFT JOIN periodo_escolar pe
                                          ON pe.id = ft.periodo_escolar_id
                                   INNER JOIN turma t
                                           ON t.id = ft.turma_id
                                   INNER JOIN conselho_classe cc
                                           ON cc.fechamento_turma_id = ft.id
                                   INNER JOIN conselho_classe_aluno cca
                                           ON cca.conselho_classe_id = cc.id
                                   INNER JOIN conselho_classe_nota ccn
                                           ON ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                                   inner join componente_curricular ccr on ccn.componente_curricular_codigo  = ccr.id 
                                   LEFT JOIN fechamento_turma_disciplina ftd
                                          ON ftd.fechamento_turma_id = ft.id
                                   LEFT JOIN fechamento_aluno fa
                                          ON fa.fechamento_turma_disciplina_id = ftd.id
                                             AND cca.aluno_codigo = fa.aluno_codigo
                                   LEFT JOIN fechamento_nota fn
                                          ON fn.fechamento_aluno_id = fa.id
                                             AND ccn.componente_curricular_codigo = fn.disciplina_id
                            WHERE  t.turma_id = ANY(@turmasCodigos)
                                   AND cca.aluno_codigo = @alunoCodigo
                                   AND bimestre IS NULL 
                                   and ccr.permite_lancamento_nota)  x";

            return await database.Conexao.QueryAsync<NotaConceitoFechamentoConselhoFinalDto>(query, new { turmasCodigos, alunoCodigo });
        }
    }
}