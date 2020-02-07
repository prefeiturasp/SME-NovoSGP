using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurmaDisciplina : RepositorioBase<FechamentoTurmaDisciplina>, IRepositorioFechamentoTurmaDisciplina
    {
        public RepositorioFechamentoTurmaDisciplina(ISgpContext database) : base(database)
        {
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre)
        {
            var query = @"select f.*
                         from fechamento_turma_disciplina f
                        inner join periodo_fechamento_bimestre b on b.id = f.periodo_fechamento_bimestre_id
                        inner join periodo_escolar p on p.id = b.periodo_escolar_id
                        where not f.excluido
                          and f.turma_id = @turmaId
                          and f.disciplina_id = @disciplinaId
                          and p.bimestre = @bimestre ";

            return await database.Conexao.QueryFirstAsync<FechamentoTurmaDisciplina>(query, new { turmaId, disciplinaId, bimestre });
        }

        public async Task<IEnumerable<NotaConceitoBimestreDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId)
        {
            var query = @"select n.disciplina_id as DisciplinaId, n.nota as Nota, n.conceito_id as ConceitoId
                         from nota_conceito_bimestre n 
                        where not f.excluido
                            and n.fechamento_turma_disciplina_id = @fechamentoTurmaId
                            and n.codigo_aluno = @codigoAluno ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreDto>(query, new { codigoAluno, fechamentoTurmaId });
        }
    }
}