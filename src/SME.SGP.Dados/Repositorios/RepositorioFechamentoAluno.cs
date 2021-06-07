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

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, long fechamentoTurmaId)
        {
            var query = new StringBuilder();

            query.AppendLine("select fa.anotacao, ftd.disciplina_id,");
            query.AppendLine("  case");
            query.AppendLine("    when fa.alterado_por is null then fa.criado_por");
            query.AppendLine("    when fa.alterado_por is not null then fa.alterado_por");
            query.AppendLine("  end as professor,");
            query.AppendLine("  case");
            query.AppendLine("    when fa.alterado_em is null then fa.criado_em");
            query.AppendLine("    when fa.alterado_em is not null then fa.alterado_em");
            query.AppendLine("  end as data,");
            query.AppendLine("  case ");
            query.AppendLine("    when fa.alterado_rf is null then fa.criado_rf");
            query.AppendLine("    when fa.alterado_rf is not null then fa.alterado_rf");
            query.AppendLine("  end as professorrf");
            query.AppendLine("from fechamento_turma_disciplina ftd ");
            query.AppendLine("inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id");
            query.AppendLine("where not ftd.excluido and not fa.excluido ");
            query.AppendLine(" and fa.aluno_codigo = @alunoCodigo and ftd.fechamento_turma_id = @fechamentoTurmaId");
            query.AppendLine(" and fa.anotacao is not null");

            return await database.Conexao.QueryAsync<FechamentoAlunoAnotacaoConselhoDto>(query.ToString(), new { alunoCodigo, fechamentoTurmaId });
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