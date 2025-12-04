using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFechamentoAlunoConsulta : RepositorioBase<AnotacaoFechamentoAluno>, IRepositorioAnotacaoFechamentoAlunoConsulta
    {
        public RepositorioAnotacaoFechamentoAlunoConsulta(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<AnotacaoFechamentoAluno> ObterPorFechamentoEAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select aaf.* 
                          from anotacao_fechamento_aluno aaf 
                         inner join fechamento_aluno fa on fa.id = aaf.fechamento_aluno_id
                         where not fa.excluido 
                           and fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                           and fa.aluno_codigo = @alunoCodigo";

            return database.Conexao.QueryFirstOrDefaultAsync<AnotacaoFechamentoAluno>(query, new { fechamentoTurmaDisciplinaId, alunoCodigo });
        }

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, string[] turmasCodigos, long periodoId)
        {
            var query = @"select coalesce(afa.anotacao, fa.anotacao) as Anotacao, ftd.disciplina_id as DisciplinaId,
                          case
                            when afa.alterado_por is null then afa.criado_por
                            when afa.alterado_por is not null then afa.alterado_por
                          end as professor,
                          case
                            when afa.alterado_em is null then afa.criado_em
                            when afa.alterado_em is not null then afa.alterado_em
                          end as data,
                          case 
                            when afa.alterado_rf is null then afa.criado_rf
                            when afa.alterado_rf is not null then afa.alterado_rf
                          end as professorrf
                        from fechamento_turma_disciplina ftd 
                        inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                        inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id 
                        inner join anotacao_fechamento_aluno afa on afa.fechamento_aluno_id = fa.id
                        inner join turma t on ft.turma_id = t.id 
                        where not ftd.excluido and not afa.excluido 
                         and fa.aluno_codigo = @alunoCodigo
                         and ft.periodo_escolar_id = @periodoId    
                         and t.turma_id =  ANY(@turmasCodigos)
                         and afa.anotacao is not null;";

            return await database.Conexao.QueryAsync<FechamentoAlunoAnotacaoConselhoDto>(query.ToString(), new { alunoCodigo, turmasCodigos, periodoId });
        }

        public Task<IEnumerable<string>> ObterAlunosComAnotacaoNoFechamento(long fechamentoTurmaDisciplinaId)
        {
            var query = @"select fa.aluno_codigo  
                          from fechamento_aluno fa 
                        inner join anotacao_fechamento_aluno afa on afa.fechamento_aluno_id = fa.id
                         where not fa.excluido 
                           and fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";

            return database.Conexao.QueryAsync<string>(query, new { fechamentoTurmaDisciplinaId });
        }

        public async Task<IEnumerable<AnotacaoFechamentoAluno>> ObterPorFechamentoEAluno(long[] fechamentosTurmasDisciplinasIds, string[] alunosCodigos)
        {
            var query = @"select aaf.*, fa.*, ftd.*
                          from anotacao_fechamento_aluno aaf 
                         inner join fechamento_aluno fa on fa.id = aaf.fechamento_aluno_id
                         inner join fechamento_turma_disciplina ftd on ftd.id = fa.fechamento_turma_disciplina_id
                         where not fa.excluido 
                           and fa.fechamento_turma_disciplina_id = any(@fechamentosTurmasDisciplinasIds)
                           and fa.aluno_codigo = any(@alunosCodigos)";

            return await database.Conexao.QueryAsync<AnotacaoFechamentoAluno, FechamentoAluno, FechamentoTurmaDisciplina, AnotacaoFechamentoAluno>(query, (anotacaoFechamentoAluno, fechamentoAluno, fechamentoTurmaDisciplina) =>
            {
                anotacaoFechamentoAluno.FechamentoAluno = fechamentoAluno;
                anotacaoFechamentoAluno.FechamentoAluno.FechamentoTurmaDisciplina = fechamentoTurmaDisciplina;
                return anotacaoFechamentoAluno;
            }, new { fechamentosTurmasDisciplinasIds, alunosCodigos }, splitOn: "id, id, id");
        }
    }
}
