using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAula : RepositorioBase<PlanoAula>, IRepositorioPlanoAula
    {
        public RepositorioPlanoAula(ISgpContext conexao) : base(conexao) { }

        public async Task ExcluirPlanoDaAula(long aulaId)
        {
            // Excluir objetivos de aprendizagem do plano
            var command = @"update objetivo_aprendizagem_aula 
                                set excluido = true
                            where plano_aula_id in (
                                select id from plano_aula 
                                 where not excluido and aula_id = @aulaId) ";
            await database.ExecuteAsync(command, new { aulaId });

            // Excluir plano de aula
            command = "update plano_aula set excluido = true where not excluido and aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task<PlanoAula> ObterPlanoAulaPorAula(long aulaId)
        {
            var query = "select * from plano_aula where not excluido and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { aulaId });
        }

        public async Task<PlanoAula> ObterPlanoAulaPorDataDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            var query = @"select pa.*
                 from plano_aula pa
                inner join aula a on a.Id = pa.aula_id
                where not a.excluido 
                  and not pa.excluido
                  and DATE(a.data_aula) = @data
                  and a.turma_id = @turmaId
                  and a.disciplina_id = @disciplinaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { data, turmaId, disciplinaId });
        }

        public async Task<bool> PlanoAulaRegistradoAsync(long aulaId)
        {
            var query = "select 1 from plano_aula where aula_id = @aulaId";
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { aulaId });
        }

        public bool ValidarPlanoExistentePorTurmaDataEDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            var query = @"select
	                            1
                            from
	                            plano_aula pa
                             inner join aula a on a.Id = pa.aula_id
                             where DATE(a.data_aula) = @data
                              and a.turma_id = @turmaId
                              and a.disciplina_id = @disciplinaId";

            return database.Conexao.Query<bool>(query, new { data = data.Date, turmaId, disciplinaId }).SingleOrDefault();
        }
    }
}
