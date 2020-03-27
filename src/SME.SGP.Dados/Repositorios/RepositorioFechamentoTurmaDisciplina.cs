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
    public class RepositorioFechamentoTurmaDisciplina : RepositorioBase<FechamentoTurmaDisciplina>, IRepositorioFechamentoTurmaDisciplina
    {
        public RepositorioFechamentoTurmaDisciplina(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId, string[] disciplinasId, int bimestre)
        {
            var query = new StringBuilder(@"select f.*
                         from fechamento_turma_disciplina f
                        inner join periodo_fechamento_bimestre b on b.id = f.periodo_fechamento_bimestre_id
                        inner join periodo_escolar p on p.id = b.periodo_escolar_id
                        inner join turma t on t.id = f.turma_id
                        where not f.excluido
                            and t.id = @turmaId
                            and p.bimestre = @bimestre ");

            if (disciplinasId != null && disciplinasId.Length > 0)
                query.AppendLine("and f.disciplina_id = ANY(@disciplinaId)");

            return await database.Conexao.QueryAsync<FechamentoTurmaDisciplina>(query.ToString(), new { turmaId, disciplinasId, bimestre });
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre)
        {
            var query = @"select f.*
                         from fechamento_turma_disciplina f
                        inner join periodo_escolar p on p.id = f.periodo_escolar_id
                        inner join turma t on t.id = f.turma_id
                        where not f.excluido
                          and t.turma_id = @turmaId
                          and f.disciplina_id = @disciplinaId
                          and p.bimestre = @bimestre ";

            var fechamentos = await database.Conexao.QueryAsync<FechamentoTurmaDisciplina>(query, new { turmaId, disciplinaId, bimestre });
            if (fechamentos == null || !fechamentos.Any())
                return null;

            return fechamentos.First();
        }

        public async Task<IEnumerable<NotaConceitoBimestreDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
        {
            var query = @"select n.disciplina_id as DisciplinaId, n.nota as Nota, n.conceito_id as ConceitoId, n.sintese_id as SinteseId
                         from nota_conceito_bimestre n
                        where not n.excluido
                            and n.fechamento_turma_disciplina_id = @fechamentoTurmaId
                            and n.codigo_aluno = @codigoAluno ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreDto>(query, new { codigoAluno, fechamentoTurmaId });
        }
    }
}