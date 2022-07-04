using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFechamentoAluno : RepositorioBase<AnotacaoFechamentoAluno>, IRepositorioAnotacaoFechamentoAluno
    {
        public RepositorioAnotacaoFechamentoAluno(ISgpContext database) : base(database)
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
            var query = @"select coalesce(afa.anotacao, fa.anotacao) as Anotacao, ftd.disciplina_id,
                          case
                            when fa.alterado_por is null then fa.criado_por
                            when fa.alterado_por is not null then fa.alterado_por
                          end as professor,
                          case
                            when fa.alterado_em is null then fa.criado_em
                            when fa.alterado_em is not null then fa.alterado_em
                          end as data,
                          case 
                            when fa.alterado_rf is null then fa.criado_rf
                            when fa.alterado_rf is not null then fa.alterado_rf
                          end as professorrf
                        from fechamento_turma_disciplina ftd 
                        inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
                        inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id 
                        inner join anotacao_fechamento_aluno afa on afa.fechamento_aluno_id = fa.id
                        inner join turma t on ft.turma_id = t.id 
                        where not ftd.excluido and not fa.excluido 
                         and fa.aluno_codigo = @alunoCodigo
                         and ft.periodo_escolar_id   = @periodoId    
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
    }
}
