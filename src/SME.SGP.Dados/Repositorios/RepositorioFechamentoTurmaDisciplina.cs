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

        public async Task<IEnumerable<NotaConceitoBimestreDto>> ObterNotasBimestre(string codigoAluno, long disciplinaId, int bimestre)
        {
            var query = @"select n.disciplina_id as DisciplinaId, n.nota as Nota, n.conceito_id as ConceitoId
                         from fechamento_turma_disciplina f
                        inner join nota_conceito_bimestre n on n.fechamento_turma_disciplina_id = f.id
                        inner join periodo_fechamento_bimestre b on b.id = f.periodo_fechamento_bimestre_id
                        inner join periodo_escolar p on p.id = b.periodo_escolar_id
                        where not f.excluido
                            and n.codigo_aluno = @codigoAluno
                            and f.disciplina_id = @disciplinaId
                            and p.bimestre = @bimestre ";

            return await database.Conexao.QueryAsync<NotaConceitoBimestreDto>(query, new { codigoAluno, disciplinaId, bimestre});
        }
    }
}