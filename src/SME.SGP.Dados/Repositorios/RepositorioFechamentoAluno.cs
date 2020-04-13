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

        public async Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, string turmaCodigo, int bimestre, bool EhFinal)
        {
            var query = new StringBuilder();

            query.AppendLine("select fa.anotacao, ftd.disciplina_id,");
            query.AppendLine("case");
            query.AppendLine("when fa.alterado_por is null then fa.criado_por");
            query.AppendLine("when fa.alterado_por is not null then fa.alterado_por");
            query.AppendLine("end as professor,");
            query.AppendLine("case");
            query.AppendLine("when fa.alterado_em is null then fa.criado_em");
            query.AppendLine("when fa.alterado_em is not null then fa.alterado_em");
            query.AppendLine("end as data,");
            query.AppendLine("case ");
            query.AppendLine("when fa.alterado_rf is null then fa.criado_rf");
            query.AppendLine("when fa.alterado_rf is not null then fa.alterado_rf");
            query.AppendLine("end as professorrf");
            query.AppendLine("from fechamento_turma ft");
            query.AppendLine("inner join fechamento_turma_disciplina ftd");
            query.AppendLine("on ftd.fechamento_turma_id = ft.id");
            query.AppendLine("inner join fechamento_aluno fa");
            query.AppendLine("on fa.fechamento_turma_disciplina_id = ftd.id");
            if (!EhFinal)
            {
                query.AppendLine("inner join periodo_escolar pe");
                query.AppendLine("on ft.periodo_escolar_id = pe.id");
            }
            query.AppendLine("inner join turma t");
            query.AppendLine("on ft.turma_id = t.id");
            query.AppendLine("where");
            query.AppendLine("fa.aluno_codigo = @alunoCodigo");
            query.AppendLine("and t.turma_id = @turmaCodigo");

            if (EhFinal)
                query.AppendLine("and ft.periodo_escolar_id is null");
            else
                query.AppendLine("and pe.bimestre = @bimestre");

            return await database.Conexao.QueryAsync<FechamentoAlunoAnotacaoConselhoDto>(query.ToString(), new { alunoCodigo, turmaCodigo, bimestre });
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

            return (await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    fechamentoAluno.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId, alunoCodigo })).FirstOrDefault();
        }

        public async Task<IEnumerable<FechamentoAluno>> ObterPorFechamentoTurmaDisciplina(long fechamentoTurmaDisciplinaId)
        {
            var query = @"select fa.*, n.*
                            from fechamento_aluno fa
                           inner join fechamento_nota n on n.fechamento_aluno_id = fa.id
                           where fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId";

            return await database.Conexao.QueryAsync<FechamentoAluno, FechamentoNota, FechamentoAluno>(query
                , (fechamentoAluno, fechamentoNota) =>
                {
                    fechamentoAluno.FechamentoNotas.Add(fechamentoNota);
                    return fechamentoAluno;
                }
                , new { fechamentoTurmaDisciplinaId });
        }
    }
}