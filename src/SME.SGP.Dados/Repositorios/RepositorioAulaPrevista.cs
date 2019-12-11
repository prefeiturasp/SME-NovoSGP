using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAulaPrevista : RepositorioBase<AulaPrevista>, IRepositorioAulaPrevista
    {
        public RepositorioAulaPrevista(ISgpContext conexao) : base(conexao)
        {
        }

        public string ObterProfessorTurmaDisciplinaAulasPrevistasDivergente(int bimestre, string turmaId, string disciplinaId, int limiteDias)
        {
            var query = @"select a.professor_rf
                          from aula a
                         inner join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id and  a.data_aula BETWEEN p.periodo_inicio AND p.periodo_fim
                         where not a.excluido
                           and now() between p.periodo_inicio and p.periodo_fim
                           and DATE_PART('day', age(p.periodo_fim, date(now()))) <= @limiteDias
                           and p.bimestre = @bimestre
                           and a.turma_id = @turmaId
                           and a.disciplina_id = @disciplinaId
                          ORDER BY a.data_aula DESC NULLS LAST LIMIT 1";

            return database.Conexao.QuerySingleOrDefault<string>(query, new { bimestre, turmaId, disciplinaId, limiteDias });
        }
    }
}
