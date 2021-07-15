using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoAulaPrevista : RepositorioBase<NotificacaoAulaPrevista>, IRepositorioNotificacaoAulaPrevista
    {
        public RepositorioNotificacaoAulaPrevista(ISgpContext database) : base(database)
        {
        }

        //Não retorna modalidade infantil
        public async Task<IEnumerable<RegistroAulaPrevistaDivergenteDto>> ObterTurmasAulasPrevistasDivergentes(int limiteDias)
        {
            var query = @"select a.turma_id as CodigoTurma, t.nome as NomeTurma
	                        , ue.ue_id as CodigoUe, ue.nome as NomeUe
	                        , dre.dre_id as CodigoDre, dre.nome as NomeDre
                            , a.disciplina_id as DisciplinaId, a.professor_rf as ProfessorRf, pe.bimestre
                           from aula a
                          inner join turma t on t.turma_id = a.turma_id
                          inner join ue on ue.id = t.ue_id
                          inner join dre on dre.id = ue.dre_id
                          inner join periodo_escolar pe on a.tipo_calendario_id = pe.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim  
                           left join aula_prevista ap 
                                on ap.tipo_calendario_id = pe.tipo_calendario_id 
                                and ap.turma_id = a.turma_id 
                                and ap.disciplina_id = a.disciplina_id
                           left join aula_prevista_bimestre apb 
                                on ap.id = apb.aula_prevista_id 
                                and pe.bimestre = apb.bimestre
                         where not a.excluido
                           and now() between pe.periodo_inicio and pe.periodo_fim
                           and DATE_PART('day', age(pe.periodo_fim, date(now()))) <= @limiteDias
                           and t.modalidade_codigo <> 1
                           and a.tipo_aula = 1

                         group by
                         	a.turma_id, t.nome, ue.ue_id , ue.nome, dre.dre_id, dre.nome, a.disciplina_id,  a.professor_rf, pe.bimestre, apb.aulas_previstas
                         having COUNT(a.id) <> coalesce(apb.aulas_previstas, 0)
                        order by dre.dre_id, ue.ue_id, a.turma_id";

            return await database.Conexao.QueryAsync<RegistroAulaPrevistaDivergenteDto>(query, new { limiteDias });
        }

        public async Task<bool> UsuarioNotificado(long usuarioId, int bimestre, string turmaId, string disciplinaId)
        {
            var query = @"select 0 
                          from notificacao_aula_prevista a
                         inner join notificacao n on n.codigo = a.notificacao_id
                         where n.usuario_id = @usuarioId
                           and a.bimestre = @bimestre
                           and a.disciplina_id = @disciplinaId
                           and a.turma_id = @turmaId";

            return (await database.Conexao.QueryAsync<int>(query, new { usuarioId, bimestre, disciplinaId, turmaId }))
                .Any();
        }
    }
}
