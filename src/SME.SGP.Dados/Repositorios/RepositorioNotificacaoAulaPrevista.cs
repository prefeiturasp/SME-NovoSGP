using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoAulaPrevista : IRepositorioNotificacaoAulaPrevista
    {
        private readonly ISgpContext database;

        public RepositorioNotificacaoAulaPrevista(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<RegistroAulaPrevistaDivergenteDto>> ObterTurmasAulasPrevistasDivergentes(int limiteDias)
        {
            try
            {
                var query = @"select distinct a.turma_id as CodigoTurma, t.nome as NomeTurma
	                        , ue.ue_id as CodigoUe, ue.nome as NomeUe
	                        , dre.dre_id as CodigoDre, dre.nome as NomeDre
                            , a.disciplina_id as DisciplinaId, pe.bimestre
                           from aula a
                          inner join turma t on t.turma_id = a.turma_id
                          inner join ue on ue.id = t.ue_id
                          inner join dre on dre.id = ue.dre_id
                          inner join periodo_escolar pe on a.tipo_calendario_id = pe.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim  
                          left join aula_prevista ap on pe.tipo_calendario_id = ap.tipo_calendario_id and pe.bimestre = ap.bimestre
                         where not a.excluido 
                           and now() between pe.periodo_inicio and pe.periodo_fim
                           and DATE_PART('day', age(pe.periodo_fim, date(now()))) <= @limiteDias
                         group by
                         	a.turma_id, t.nome, ue.ue_id , ue.nome, dre.dre_id, dre.nome, ap.aulas_previstas, a.disciplina_id, pe.bimestre
                         having  COUNT(a.*) filter (where a.tipo_aula = 1) <> coalesce(ap.aulas_previstas, 0)
                        order by dre.dre_id, ue.ue_id, a.turma_id";

                return await database.Conexao.QueryAsync<RegistroAulaPrevistaDivergenteDto>(query, new { limiteDias });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
