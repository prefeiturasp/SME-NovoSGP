using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoAluno : RepositorioBase<FechamentoAluno>, IRepositorioFechamentoAluno
    {
        public RepositorioFechamentoAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, string[] turmasCodigos, long periodoId)
        {
            var query = @"select fa.anotacao, ftd.disciplina_id,
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
                        inner join turma t on ft.turma_id = t.id 
                        where not ftd.excluido and not fa.excluido 
                         and fa.aluno_codigo = @alunoCodigo
                         and ft.periodo_escolar_id   = @periodoId    
                         and t.turma_id =  ANY(@turmasCodigos)
                         and fa.anotacao is not null;";

            return await database.Conexao.QueryAsync<FechamentoAlunoAnotacaoConselhoDto>(query.ToString(), new { alunoCodigo, turmasCodigos, periodoId   });
        }

        public async Task<FechamentoAluno> ObterFechamentoAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select *
                        from fechamento_aluno
                        where not excluido
                          and fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                          and aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoAluno>(query, new { fechamentoTurmaDisciplinaId, alunoCodigo });
        }

        public async Task<FechamentoAluno> ObterFechamentoAlunoENotas(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select a.*, n.*
                            from fechamento_aluno a
                           inner join fechamento_nota n on n.fechamento_aluno_id = a.id
                           where not a.excluido
                             and a.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                             and a.aluno_codigo = @alunoCodigo";

            FechamentoAluno fechamentoAlunoRetorno = null;
            await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    if (fechamentoAlunoRetorno == null)
                        fechamentoAlunoRetorno = fechamentoAluno;

                    fechamentoAlunoRetorno.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId, alunoCodigo });

            return fechamentoAlunoRetorno;
        }

        public async Task<IEnumerable<FechamentoAluno>> ObterPorFechamentoTurmaDisciplina(long fechamentoTurmaDisciplinaId)
        {
            var query = @"select fa.*, n.*
                            from fechamento_aluno fa
                           inner join fechamento_nota n on n.fechamento_aluno_id = fa.id
                           where fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";

            List<FechamentoAluno> fechamentosAlunos = new List<FechamentoAluno>();

            await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    var fechamentoAlunoLista = fechamentosAlunos.FirstOrDefault(a => a.Id == fechamentoAluno.Id);
                    if (fechamentoAlunoLista == null)
                    {
                        fechamentoAlunoLista = fechamentoAluno;
                        fechamentosAlunos.Add(fechamentoAluno);
                    }
                    fechamentoAlunoLista.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId });

            return fechamentosAlunos;
        }
    }
}