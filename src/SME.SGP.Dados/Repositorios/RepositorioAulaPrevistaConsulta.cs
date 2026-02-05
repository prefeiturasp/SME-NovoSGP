using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAulaPrevistaConsulta : RepositorioBase<AulaPrevista>, IRepositorioAulaPrevistaConsulta
    {
        public RepositorioAulaPrevistaConsulta(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<AulaPrevista> ObterAulaPrevistaFiltro(long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            var query = @"select * from aula_prevista ap
                         where ap.tipo_calendario_id = @tipoCalendarioId and ap.turma_id = @turmaId and
                               ap.disciplina_id = @disciplinaId and 
                               not ap.excluido;";

            return await database.Conexao.QueryFirstOrDefaultAsync<AulaPrevista>(query, new { tipoCalendarioId, turmaId, disciplinaId });
        }

        public async Task<IEnumerable<AulaPrevista>> ObterAulasPrevistasPorUe(long codigoUe)
        {
            var query = @"select ap.* from aula_prevista ap 
                            join turma t on t.turma_id = ap.turma_id
                            where t.ue_id = @codigoUe 
                            and not ap.excluido";

            return await database.Conexao.QueryAsync<AulaPrevista>(query, new { codigoUe });
        }


        public string ObterProfessorTurmaDisciplinaAulasPrevistasDivergente(int bimestre, string turmaId, string disciplinaId, int limiteDias)
        {
            var query = @"select a.professor_rf
                          from aula a
                         inner join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id and  a.data_aula BETWEEN p.periodo_inicio AND p.periodo_fim
                         inner join tipo_calendario tp on a.tipo_calendario_id = tp.id
                         where (a.id is null or not a.excluido) 
                           and tp.situacao and not tp.excluido 
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
